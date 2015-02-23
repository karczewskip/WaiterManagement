using System;
using System.Collections.Generic;
using System.Linq;
using ClassLib.DbDataStructures;
using System.Security;
using System.Data.Entity;
using System.ServiceModel;
using System.Threading.Tasks;
using DataAccess.Migrations;
using ClassLib.DataStructures;
using ClassLib.ServiceContracts;

namespace DataAccess
{
    /// <summary>
    /// Klasa agregująca metody dostępu do bazy danych
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext=false, IncludeExceptionDetailInFaults = true)]
    public class DataAccessClass : IManagerDataAccessWCFService, IWaiterDataAccessWCFService, IClientDataAccessWCFService, IDataWipe, IManagerDataAccess, IWaiterDataAccess, IClientDataAccess, IClientDataAccessWebWCFService, IManagerDataAccessWebWCFService
    {
        #region Const Fields
        /// <summary>
        /// Przerwa pomiędzy kolejnymi wywołaniami schedulera
        /// </summary>
        private const long schedulerInterval = 1000*10;
        #endregion

        #region Private Fields
        private Logger logger;

        private HashSet<UserContextEntity> loggedInUsers;
        private object loggedInUsersLockObject = new object();
        private Queue<Order> awaitingOrderCollection; 
        private object awaitingOrderCollectionLockObject = new object();
        private Dictionary<WaiterRegistrationRecord, Order> waiterOrderDictionary;
        private object waiterOrderDictionaryLockObject = new object();
        private HashSet<ClientRegistrationRecord> clientRegistrationRecords;
        private object clientRegistrationRecordsLockObject = new object();
        private OrderAssignScheduler orderAssignScheduler;
        #endregion

        #region Constructors
        public DataAccessClass()
        {
            logger = new Logger();

            loggedInUsers = new HashSet<UserContextEntity>();
            awaitingOrderCollection = new Queue<Order>();
            waiterOrderDictionary = new Dictionary<WaiterRegistrationRecord, Order>();
            clientRegistrationRecords = new HashSet<ClientRegistrationRecord>();

            orderAssignScheduler = new OrderAssignScheduler( schedulerInterval, FindTableAndAssignOrder);

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataAccessProvider, Configuration>()); 
        }
        #endregion

        #region IBaseDataAccess
        /// <summary>
        /// Zwraca kolekcję kategorii elementów menu
        /// </summary>
        /// <param name="userId">Identyfikator użytkownika</param>
        /// <returns></returns>
        public IEnumerable<MenuItemCategory> GetMenuItemCategories(int userId)
        {
            if (!CheckIsUserLoggedIn(userId))
            {
                string errorMessage = String.Format("User login={0} is not logged in", userId);
                logger.Write(errorMessage, LoggingCategory.Warning);
                throw new SecurityException(errorMessage);
            }

            using (var db = new DataAccessProvider())
            {
                var menuItemCategoryList = db.MenuItemCategories.Where( m => !m.IsDeleted).ToList();
                return menuItemCategoryList.Select(c => new MenuItemCategory(c)).ToList();
            }
        }

        /// <summary>
        /// Zwraca kolekcję elementów menu
        /// </summary>
        /// <param name="userId">Identyfikator uzytkownika</param>
        /// <returns></returns>
        public IEnumerable<MenuItem> GetMenuItems(int userId)
        {
            if (!CheckIsUserLoggedIn(userId))
            {
                string errorMessage = String.Format("User login={0} is not logged in", userId);
                logger.Write(errorMessage, LoggingCategory.Warning);
                throw new SecurityException(errorMessage);
            }

            return GetMenuItems();
        }

        /// <summary>
        /// Zwraca kolekcję elementów menu
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MenuItem> GetMenuItems()
        {
            using (var db = new DataAccessProvider())
            {
                var menuItemList = db.MenuItems.Include("Category").Where(mI => !mI.IsDeleted).ToList();
                return menuItemList.Select(m => new MenuItem(m)).ToList();
            }
        }

        /// <summary>
        /// Zwraca kolekcję stolików zarejestrowanych w systemie
        /// </summary>
        /// <param name="userId">Identyfikator klienta</param>
        /// <returns></returns>
        public IEnumerable<Table> GetTables(int userId)
        {
            if (!CheckIsUserLoggedIn(userId))
            {
                string errorMessage = String.Format("User login={0} is not logged in", userId);
                logger.Write(errorMessage, LoggingCategory.Warning);
                throw new SecurityException(errorMessage);
            }

            using (var db = new DataAccessProvider())
            {
                var tableList = db.Tables.Where( t => !t.IsDeleted).ToList();
                return tableList.Select(t => new Table(t)).ToList();
            }
        }

        /// <summary>
        /// Metoda logująca użytkownika systemu.
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="password">Hasło użytkownika</param>
        /// <remarks>Aby poprawnie przeprowadzić logowanie, haslo podane w tej metodzie musi być w postaci pierwszego hasza (haszowane klasą <href>HashClass</href>)</remarks>
        /// <returns></returns>
        UserContext IBaseDataAccessWCFService.LogIn(string login, string password)
        {
            if (String.IsNullOrEmpty(login))
                throw new ArgumentNullException("login");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            CheckAreClientsAvailable();
            CheckAreWaitersAvailable();

            UserContextEntity userContextEntity = null;

            using (var db = new DataAccessProvider())
            {
                userContextEntity = db.Users.FirstOrDefault(u => u.Login.Equals(login));
                if (userContextEntity == null)
                    return null;

                var userPassword = db.Passwords.FirstOrDefault(pw => pw.UserId == userContextEntity.Id);
                if (userPassword == null)
                    return null;

                if (String.IsNullOrEmpty(userPassword.Hash))
                    return null;

                if(!HashClass.ValidatePassword(password, userPassword.Hash))
                    return null;
            }

            if (!CheckIsUserLoggedIn(userContextEntity.Id))
            {
                lock(loggedInUsersLockObject)
                    loggedInUsers.Add(userContextEntity);
            }

            UserContext userContext = new UserContext(userContextEntity);

            if (userContextEntity.Role == UserRole.Client)
                lock(clientRegistrationRecordsLockObject)
                    clientRegistrationRecords.Add(new ClientRegistrationRecord(userContextEntity.Id,
                        OperationContext.Current.GetCallbackChannel<IClientDataAccessCallbackWCFService>()));
            else if (userContextEntity.Role == UserRole.Waiter)
            {
                lock(waiterOrderDictionary)
                    waiterOrderDictionary.Add(
                        new WaiterRegistrationRecord(userContextEntity.Id,
                            OperationContext.Current.GetCallbackChannel<IWaiterDataAccessCallbackWCFService>()), null);

                Task.Run(() => TryAssignAwaitingOrder(userContext));
            }

            logger.Write(String.Format("User {0}({1}) logged in.", userContextEntity.Login, userContextEntity.Role), LoggingCategory.Information);
            OperationContext.Current.Channel.Faulted += Channel_Faulted;

            return userContext;
        }

        /// <summary>
        /// Przechwytywanie zdarzenia przejścia kanału WCF-owego w stan Faulted. Póki  co raczej nie jest wołana.....
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Channel_Faulted(object sender, EventArgs e)
        {
            IContextChannel channel = sender as IContextChannel;

            logger.Write(String.Format("Channel {0} entered faulted state. {1}", channel.GetType(), e), LoggingCategory.Error);
        }
        
        /// <summary>
        /// Metoda logująca użytkownika w systemia.
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="password">Hasło uzytkownika</param>
        /// <remarks>Aby poprawnie przeprowadzić logowanie, haslo podane w tej metodzie musi być w postaci pierwszego hasza (haszowane klasą <href>HashClass</href>)</remarks>
        /// <returns></returns>
        public UserContext 
            LogIn(string login, string password)
        {
            if (String.IsNullOrEmpty(login))
                throw new ArgumentNullException("login");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            UserContextEntity userContextEntity = null;

            using (var db = new DataAccessProvider())
            {
                userContextEntity = db.Users.FirstOrDefault(u => u.Login.Equals(login));
                if (userContextEntity == null)
                    return null;

                var userPassword = db.Passwords.FirstOrDefault(pw => pw.UserId == userContextEntity.Id);
                if (userPassword == null)
                    return null;

                if (String.IsNullOrEmpty(userPassword.Hash))
                    return null;

                if (!HashClass.ValidatePassword(password, userPassword.Hash))
                    return null;
            }

            if (!CheckIsUserLoggedIn(userContextEntity.Id))
            {
                lock (loggedInUsersLockObject)
                    loggedInUsers.Add(userContextEntity);
            }

            logger.Write(String.Format("User {0}({1}) logged in.", userContextEntity.Login, userContextEntity.Role), LoggingCategory.Information);
            return new UserContext(userContextEntity);
        }

        /// <summary>
        /// Metoda wylogowująca użytkownika ze systemu
        /// </summary>
        /// <param name="userId">Identyfikator użytkownika</param>
        /// <returns></returns>
        public bool LogOut(int userId)
        {
            if (!CheckIsUserLoggedIn(userId))
                return false;

            return RemoveFromLoggedInUsers(userId);
        }

        #endregion

        #region IManagerDataAccess
        /// <summary>
        /// Rejestruje w systemie użytkownika o roli menedżera baru.
        /// </summary>
        /// <param name="firstName">Imie użytkownika</param>
        /// <param name="lastName">Nazwisko użytkownika</param>
        /// <param name="login">Login użytkownika</param>
        /// <param name="password">Hasło użytkownika</param>
        /// <remarks>Aby poprawnie przeprowadzić rejestrowanie, haslo podane w tej metodzie musi być w postaci pierwszego hasza (haszowane klasą <href>HashClass</href>)</remarks>
        /// <returns></returns>
        public UserContext AddManager(string firstName, string lastName, string login, string password)
        {
            return AddUserToDatabase(firstName, lastName, login, password, UserRole.Manager);
        }

        /// <summary>
        /// Rejestruje w systemie kategorię elementów menu.
        /// </summary>
        /// <param name="managerId">Identyfikator użytkownika o roli menedżera baru.</param>
        /// <param name="name">Nazwa kategorii</param>
        /// <param name="description">Opis kategorii</param>
        /// <returns></returns>
        public MenuItemCategory AddMenuItemCategory(int managerId, string name, string description)
        {
            if (!CheckHasUserRole(managerId, UserRole.Manager))
                throw new SecurityException(String.Format("User id = {0} is not logged in or is no manager", managerId));

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("Name is null");
            if (String.IsNullOrEmpty(description))
                throw new ArgumentNullException("Description is null");           

            MenuItemCategoryEntity newCategoryEntity = null;

            using( var db = new DataAccessProvider())
            {
                var categoryToAddEntity = new MenuItemCategoryEntity() {Name = name, Description = description};

                var categoriesSameName = db.MenuItemCategories.Where(c => c.Name.Equals(name));
                
                if(categoriesSameName != null && categoriesSameName.Any())
                    foreach(MenuItemCategoryEntity category in categoriesSameName)
                        if(category.Equals(categoryToAddEntity))
                        {
                            if(category.IsDeleted)
                                category.IsDeleted = false;
                            newCategoryEntity = category;
                            break;
                        }

                if(newCategoryEntity == null)
                    newCategoryEntity = db.MenuItemCategories.Add(categoryToAddEntity);

                db.SaveChanges();
            }

            return new MenuItemCategory(newCategoryEntity);
        }

        /// <summary>
        /// Edytuje kategorię elementów menu
        /// </summary>
        /// <param name="managerId">Identyfikator użytkownika o roli menedżera systemu</param>
        /// <param name="menuItemCategoryToEdit">Kategoria elementów menu do edycji</param>
        /// <remarks>Identyfikator kategorii musi być taki sam jak identyfikator kategorii do edycji. Pozostałe pola mogą być zmienione.</remarks>
        /// <returns></returns>
        public bool EditMenuItemCategory(int managerId, MenuItemCategory menuItemCategoryToEdit)
        {
            if (!CheckHasUserRole(managerId, UserRole.Manager))
                throw new SecurityException(String.Format("User id = {0} is not logged in or is no manager", managerId));

            if (menuItemCategoryToEdit == null)
                throw new ArgumentNullException("menuItemCategoryToEdit");           

            using(var db = new DataAccessProvider())
            {
                MenuItemCategoryEntity editedMenuItemCategoryEntity = db.MenuItemCategories.Find(menuItemCategoryToEdit.Id);
                if (editedMenuItemCategoryEntity == null || editedMenuItemCategoryEntity.IsDeleted)
                    return false;

                //db.Entry(editedMenuItemCategory).State = System.Data.Entity.EntityState.Detached;
                //db.MenuItemCategories.Attach(menuItemCategoryToEdit);
                //db.Entry(menuItemCategoryToEdit).State = System.Data.Entity.EntityState.Modified;

                editedMenuItemCategoryEntity.CopyData(menuItemCategoryToEdit);
                db.Entry(editedMenuItemCategoryEntity).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// Usuwa z systemu kategorię elementów menu
        /// </summary>
        /// <param name="managerId">Identyfikator użytkownika o roli menedżera baru</param>
        /// <param name="categoryId">Identyfikator kategorii do usunięcia</param>
        /// <returns></returns>
        public bool RemoveMenuItemCategory(int managerId, int categoryId)
        {
            if (!CheckHasUserRole(managerId, UserRole.Manager))
                throw new SecurityException(String.Format("User id = {0} is not logged in or is no manager", managerId));

            using(var db = new DataAccessProvider())
            {
                MenuItemCategoryEntity menuItemCategoryEntityToRemove = db.MenuItemCategories.Find(categoryId);
                if (menuItemCategoryEntityToRemove == null || menuItemCategoryEntityToRemove.IsDeleted)
                    return false;
                
                //db.MenuItemCategories.Remove(menuItemCategoryToRemove);
                menuItemCategoryEntityToRemove.IsDeleted = true;
                db.SaveChanges();
                return true;
            }
        }

        public MenuItem AddMenuItem(int managerId, string name, string description, int categoryId, Money price)
        {
            if (!CheckHasUserRole(managerId, UserRole.Manager))
                throw new SecurityException(String.Format("User id = {0} is not logged in or is no manager", managerId));

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");
            if (String.IsNullOrEmpty(description))
                throw new ArgumentNullException("description");

            MenuItemEntity newMenuItemEntity = null;

            using (var db = new DataAccessProvider())
            {
                MenuItemCategoryEntity category = null;

                category = db.MenuItemCategories.Find(categoryId);
                if (category == null)
                    return null;

                var menuItemToAdd = new MenuItemEntity() { Name = name, Description = description, Category = category, Price = price };

                var menuItemsSameName = db.MenuItems.Where(mI => mI.Name.Equals(name));
                if( menuItemsSameName.Any())
                    foreach(MenuItemEntity menuItem in menuItemsSameName)
                        if(menuItem.Equals(menuItemToAdd))
                        {
                            if (menuItem.IsDeleted)
                                menuItem.IsDeleted = false;

                            newMenuItemEntity = menuItem;
                            break;
                        }
                if(newMenuItemEntity == null)
                    newMenuItemEntity = db.MenuItems.Add(menuItemToAdd);

                db.SaveChanges();
            }

            return new MenuItem(newMenuItemEntity);
        }

        public bool EditMenuItem(int managerId, MenuItem menuItemToEdit)
        {
            if (!CheckHasUserRole(managerId, UserRole.Manager))
                throw new SecurityException(String.Format("User id = {0} is not logged in or is no manager", managerId));

            if (menuItemToEdit == null)
                throw new ArgumentNullException("menuItemToEdit");
            
            using(var db = new DataAccessProvider())
            {
                MenuItemEntity editedMenuItemEntity = db.MenuItems.Find(menuItemToEdit.Id);
                if (editedMenuItemEntity == null || editedMenuItemEntity.IsDeleted)
                    return false;

                //db.Entry(editedMenuItem).State = System.Data.Entity.EntityState.Detached;
                //db.MenuItems.Attach(menuItemToEdit);
                //db.Entry(menuItemToEdit).State = System.Data.Entity.EntityState.Modified;

                editedMenuItemEntity.CopyData(menuItemToEdit);
                MenuItemCategoryEntity editedMenuItemCategoryEntity =
                    db.MenuItemCategories.Find(menuItemToEdit.Category.Id);

                if (editedMenuItemCategoryEntity != null)
                    editedMenuItemEntity.Category = editedMenuItemCategoryEntity;

                db.Entry(editedMenuItemEntity).State = EntityState.Modified;

                db.SaveChanges();
                return true;
            }

        }

        public bool RemoveMenuItem(int managerId, int menuItemId)
        {
            if (!CheckHasUserRole(managerId, UserRole.Manager))
                throw new SecurityException(String.Format("User id = {0} is not logged in or is no manager", managerId));

            using(var db = new DataAccessProvider())
            {
                MenuItemEntity menuItemEntityToRemove = db.MenuItems.Find(menuItemId);
                if (menuItemEntityToRemove == null || menuItemEntityToRemove.IsDeleted)
                    return false;
                
                //db.MenuItems.Remove(menuItemToRemove);
                menuItemEntityToRemove.IsDeleted = true;
                db.SaveChanges();
                return true;
            }            
        }        

        public UserContext AddWaiter(int managerId, string firstName, string lastName, string login, string password)
        {
            if (!CheckHasUserRole(managerId, UserRole.Manager))
                throw new SecurityException(String.Format("User id = {0} is not logged in or is no manager", managerId));

            if (String.IsNullOrEmpty(firstName))
                throw new ArgumentNullException("firstName");
            if (String.IsNullOrEmpty(lastName))
                throw new ArgumentNullException("lastName");
            if (String.IsNullOrEmpty(login))
                throw new ArgumentNullException("login");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            UserContextEntity newWaiterContextEntity = null;

            using (var db = new DataAccessProvider())
            {
                var waiterContextToAdd = new UserContextEntity() { FirstName = firstName, LastName = lastName, Login = login, Role = UserRole.Waiter};
                var usersSameLogin = db.Users.Where(u => u.Role == UserRole.Waiter && u.Login.Equals(login));

                if (usersSameLogin != null && usersSameLogin.Any())
                {
                    foreach (UserContextEntity userContextEntity in usersSameLogin)
                        if (userContextEntity.Equals(waiterContextToAdd))
                        {
                            if (userContextEntity.IsDeleted)
                                userContextEntity.IsDeleted = false;

                            newWaiterContextEntity = userContextEntity;
                            break;
                        }

                    //istnieją kelnerzy o tym samym loginie, ale nie są tacy sami jak ten co chcemy dodać.
                    if(newWaiterContextEntity == null)
                        throw new ArgumentException(String.Format("login = {0} already exists in database!", login));
                }                    

                if(newWaiterContextEntity == null)
                    newWaiterContextEntity = db.Users.Add(waiterContextToAdd);
                db.SaveChanges();

                PasswordEntity newWaiterPassword = db.Passwords.FirstOrDefault(p => p.UserId == newWaiterContextEntity.Id);
                if (newWaiterPassword == null)
                {
                    newWaiterPassword = new PasswordEntity()
                    {
                        UserId = newWaiterContextEntity.Id,
                        Hash = HashClass.CreateSecondHash(password)
                    };
                    db.Passwords.Add(newWaiterPassword);
                    db.SaveChanges();
                }
                else
                {
                    newWaiterPassword.Hash = HashClass.CreateSecondHash(password);
                    db.Entry(newWaiterPassword).State = EntityState.Detached;
                    db.Passwords.Attach(newWaiterPassword);
                    db.Entry(newWaiterPassword).State = EntityState.Modified;
                    db.SaveChanges();
                }

            }

            return new UserContext(newWaiterContextEntity);
        }

        public bool EditWaiter(int managerId, UserContext waiterToEdit)
        {
            if (!CheckHasUserRole(managerId, UserRole.Manager))
                throw new SecurityException(String.Format("User id = {0} is not logged in or is no manager", managerId));

            if (waiterToEdit == null)
                throw new ArgumentNullException("waiterToEdit");

            if (waiterToEdit.Role != UserRole.Waiter)
                throw new ArgumentException(String.Format("User id = {0} is no waiter.", waiterToEdit.Id));

            using(var db = new DataAccessProvider())
            {
                UserContextEntity editedWaiterContext = db.Users.Find(waiterToEdit.Id);
                if (editedWaiterContext == null || editedWaiterContext.IsDeleted)
                    return false;

                //db.Entry(editedWaiterContext).State = System.Data.Entity.EntityState.Detached;
                //db.Users.Attach(waiterToEdit);
                //db.Entry(waiterToEdit).State = System.Data.Entity.EntityState.Modified;

                editedWaiterContext.CopyData(waiterToEdit);
                db.Entry(editedWaiterContext).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
        }

        public bool RemoveWaiter(int managerId, int waiterId)
        {
            if (!CheckHasUserRole(managerId, UserRole.Manager))
                throw new SecurityException(String.Format("User id = {0} is not logged in or is no manager", managerId));

            using(var db = new DataAccessProvider())
            {
                UserContextEntity waiterContextToRemove = db.Users.Find(waiterId);
                if (waiterContextToRemove == null || waiterContextToRemove.IsDeleted || waiterContextToRemove.Role != UserRole.Waiter)
                    return false;

                if (CheckIsUserLoggedIn(waiterContextToRemove.Id))
                    RemoveFromLoggedInUsers(waiterContextToRemove.Id);

                waiterContextToRemove.IsDeleted = true;
                db.SaveChanges();
                return true;
            }
        }

        public IEnumerable<UserContext> GetWaiters(int managerId)
        {
            if (!CheckHasUserRole(managerId, UserRole.Manager))
                throw new SecurityException(String.Format("User id = {0} is not logged in or is no manager", managerId));

            using(var db = new DataAccessProvider())
            {
                var waiterList = db.Users.Where( u => u.Role.HasFlag(UserRole.Waiter) && !u.IsDeleted ).ToList();
                return waiterList.Select(userContext => new UserContext(userContext)).ToList();
            }
        }

        public Table AddTable(int managerId, int tableNumber, string description)
        {
            if (!CheckHasUserRole(managerId, UserRole.Manager))
                throw new SecurityException(String.Format("User id = {0} is not logged in or is no manager", managerId));

            if (String.IsNullOrEmpty(description))
                throw new ArgumentNullException("description");

            TableEntity newTable = null;

            using (var db = new DataAccessProvider())
            {
                var tableToAdd = new TableEntity() { Number = tableNumber, Description = description };

                var tablesSameNumber = db.Tables.Where(t => t.Number.Equals(tableNumber));
                if(tablesSameNumber != null && tablesSameNumber.Any())
                    foreach(TableEntity tableEntity in tablesSameNumber)
                        if(tableEntity.Equals(tableToAdd))
                        {
                            if (tableEntity.IsDeleted)
                                tableEntity.IsDeleted = false;

                            newTable = tableEntity;
                            break;
                        }

                if(newTable == null)
                    newTable = db.Tables.Add(tableToAdd);

                db.SaveChanges();
            }

            return new Table(newTable);
        }

        public bool EditTable(int managerId, Table tableToEdit)
        {
            if (!CheckHasUserRole(managerId, UserRole.Manager))
                throw new SecurityException(String.Format("User id = {0} is not logged in or is no manager", managerId));

            if (tableToEdit == null)
                throw new ArgumentNullException("tableToEdit is null");

            using(var db = new DataAccessProvider())
            {
                TableEntity editedTableEntity = db.Tables.Find(tableToEdit.Id);
                if (editedTableEntity == null || editedTableEntity.IsDeleted)
                    return false;

                //db.Entry(editedTableEntity).State = System.Data.Entity.EntityState.Detached;
                //db.Tables.Attach(tableToEdit);
                //db.Entry(tableToEdit).State = System.Data.Entity.EntityState.Modified;

                editedTableEntity.CopyData(tableToEdit);
                db.Entry(editedTableEntity).State = EntityState.Modified;

                db.SaveChanges();
                return true;
            }
        }

        public bool RemoveTable(int managerId, int tableId)
        {
            if (!CheckHasUserRole(managerId, UserRole.Manager))
                throw new SecurityException(String.Format("User id = {0} is not logged in or is no manager", managerId));

            using(var db = new DataAccessProvider())
            {
                TableEntity tableEntityToRemove = db.Tables.Find(tableId);
                if(tableEntityToRemove == null || tableEntityToRemove.IsDeleted)
                    return false;

                //db.Tables.Remove(tableToRemove);
                tableEntityToRemove.IsDeleted = true;
                db.SaveChanges();
                return true;
            }
        }        

        IEnumerable<Order> IManagerDataAccess.GetOrders(int managerId)
        {
            if (!CheckHasUserRole(managerId, UserRole.Manager))
                throw new SecurityException(String.Format("User id = {0} is not logged in or is no manager", managerId));

            using(var db = new DataAccessProvider())
            {
                var orderList = db.Orders.Include("Waiter").Include("Table").Include("MenuItems").Include("Client").Where(o => !o.IsDeleted).ToList();
                return orderList.Select(o => new Order(o)).ToList();
            }
        }

        public bool RemoveOrder(int managerId, int orderId)
        {
            if (!CheckHasUserRole(managerId, UserRole.Manager))
                throw new SecurityException(String.Format("User id = {0} is not logged in or is no manager", managerId));

            using(var db = new DataAccessProvider())
            {
                OrderEntity orderToRemove = db.Orders.Find(orderId);
                if (orderToRemove == null || orderToRemove.IsDeleted)
                    return false;

                var quantityList = orderToRemove.MenuItems.ToList();
                foreach (MenuItemQuantityEntity quantity in quantityList)
                    quantity.IsDeleted = true;

                orderToRemove.IsDeleted = true;
                db.SaveChanges();
                return true;
            }
        }
        #endregion

        #region IWaiterDataAccess
        public IEnumerable<Order> GetAllPastOrders(int waiterId)
        {
            if(!CheckHasUserRole(waiterId, UserRole.Waiter))
                throw new SecurityException(String.Format("User id={0} is not logged in or is not a waiter.", waiterId));

            using(var db = new DataAccessProvider())
            {
                var orders = db.Orders.Include("MenuItems").Include("MenuItems.MenuItem").Include("MenuItems.MenuItem.Category").Include("Waiter").Include("Table").Where(o => o.Waiter.Id == waiterId && !o.IsDeleted && (o.State == OrderState.NotRealized || o.State == OrderState.Realized)).ToList();
                return orders.Select( o => new Order(o)).ToList();
            }

        }

        public IEnumerable<Order> GetPastOrders(int waiterId, int firstIndex, int lastIndex)
        {
            if (!CheckHasUserRole(waiterId, UserRole.Waiter))
                throw new SecurityException(String.Format("User id={0} is not logged in or is not a waiter.", waiterId));

            if(firstIndex < 0)
                throw new ArgumentException(String.Format("firstIndex ({0]) is smaller than zero", firstIndex));
            if(lastIndex < 0)
                throw new ArgumentException(String.Format("lastIndex ({0]) is smaller than zero", lastIndex));
            if (firstIndex > lastIndex)
                throw new ArgumentException(String.Format("firstIndex ({0}) is larger than lastIndex ({1})", firstIndex, lastIndex));

            using(var db = new DataAccessProvider())
            {
                var sortedList = db.Orders.Include("MenuItems").Include("MenuItems.MenuItem").Include("MenuItems.MenuItem.Category").Include("Waiter").Include("Table").Where(o => o.Waiter.Id == waiterId && !o.IsDeleted && (o.State == OrderState.Realized || o.State == OrderState.NotRealized)).OrderByDescending(o => o.ClosingDate).ToList();
                
                //Kelner obsłużył mniej zamówień niż pierwszy indeks
                if (sortedList.Count < firstIndex + 1)
                    return new Order[0];

                //Indeks końcowy i początkowy taki sam - zwracamy jedną wartość
                if (firstIndex == lastIndex)
                    return new Order[1] { new Order(sortedList[firstIndex]) };

                //Kelner ma mniej zamówień niż indeks końcowy - zwracamy od indeksu początkowego do końca
                if(sortedList.Count < lastIndex + 1)
                {
                    var result = new OrderEntity[sortedList.Count - firstIndex];
                    sortedList.CopyTo(firstIndex, result, 0, sortedList.Count - firstIndex);
                    return result.Select( o => new Order(o)).ToList();
                }

                var resultOrders = new OrderEntity[lastIndex - firstIndex + 1];
                sortedList.CopyTo(firstIndex, resultOrders, 0, lastIndex - firstIndex + 1);
                return resultOrders.Select( o => new Order(o)).ToList();
            }
        }

        public IEnumerable<Order> GetActiveOrders(int waiterId)
        {
            if (!CheckHasUserRole(waiterId, UserRole.Waiter))
                throw new SecurityException(String.Format("User id={0} is not logged in or is not a waiter.", waiterId));

            using(var db = new DataAccessProvider())
            {
                var activeOrders = db.Orders.Include("MenuItems").Include("MenuItems.MenuItem").Include("MenuItems.MenuItem.Category").Include("Waiter").Include("Table").Where( o => o.Waiter.Id == waiterId && !o.IsDeleted && (o.State == OrderState.Accepted || o.State == OrderState.Placed )).ToList();
                return activeOrders.Select( o => new Order(o)).ToList();
            }
        }

        public bool SetOrderState(int waiterId, int orderId, OrderState state)
        {
            if (!CheckHasUserRole(waiterId, UserRole.Waiter))
                throw new SecurityException(String.Format("User id={0} is not logged in or is not a waiter.", waiterId));

            if (state.Equals(OrderState.Placed))
                throw new ArgumentException("Cannot change Order state to Placed");

            using(var db = new DataAccessProvider())
            {
                var order = db.Orders.Include("Client").FirstOrDefault( o => o.Id == orderId);
                if (order == null)
                    throw new ArgumentException(String.Format("No such Order (id={0}) exists", orderId));


                //Nie można zmienić stan zamówienia innego kelnera
                if (order.Waiter.Id != waiterId)
                    return false;

                if (state.Equals(OrderState.Accepted))
                {
                    //Nie można zaakceptować zamówienia, który jest w stanie innym niż Placed
                    if(!order.State.Equals(OrderState.Placed))
                        return false;
                }
                else if (state.Equals(OrderState.AwaitingDelivery))
                {
                    //Nie można zakończyć przygotowanie zamówienia, które nie zostało zaakceptowane
                    if (!order.State.Equals(OrderState.Accepted))
                        return false;
                }
                else if(state.Equals(OrderState.Realized) || state.Equals(OrderState.NotRealized) )
                {
                    //Nie można zakończyć zamówienie, które nie jest w stanie AwaitingDelivery
                    if (!order.State.Equals(OrderState.AwaitingDelivery))
                        return false;

                    order.ClosingDate = DateTime.Now;
                }

                order.State = state;
                db.SaveChanges();

                if (state.Equals(OrderState.AwaitingDelivery))
                {
                    CheckAreClientsAvailable();

                    lock (clientRegistrationRecords)
                    {
                        var clientRegRec = clientRegistrationRecords.FirstOrDefault(r => r.ClientId == order.Client.Id);
                        if (clientRegRec != null)
                            Task.Run(() => clientRegRec.Callback.NotifyOrderAwaitingDelivery(order.Id));
                        //klient zamówił przez stronę internetową, co oznacza, że nie posiada interfejsu wywołań zwrotnych. Wysyłamy kelnerowi bezpośrednio potwierdzenie zapłaty.
                        else
                        {
                            Task.Run(() => PayForOrderInternal(order.Client.Id, order.Id));
                        }
                    }
                }

                return true;
            }
        }
        #endregion

        #region IClientDataAccess
        public UserContext AddClient(string firstName, string lastName, string login, string password)
        {
            return AddUserToDatabase(firstName, lastName, login, password, UserRole.Client);
        }

        /// <summary>
        /// Dodawanie zamówienia z terminala klienckiego w lokalu. Znajdywanie kelnera obsługującego zadanie następuje od razu.
        /// </summary>
        /// <param name="clientId">Identyfikator klienta</param>
        /// <param name="tableId">Numer stolika</param>
        /// <param name="menuItemsEnumerable">Elementy stanowiące zamówienie</param>
        /// <returns></returns>
        Order IClientDataAccessWCFService.AddOrder(int clientId, int tableId, IEnumerable<Tuple<int, int>> menuItemsEnumerable)
        {
            if (!CheckHasUserRole(clientId, UserRole.Client))
                throw new SecurityException(String.Format("Client id={0} is not logged in.", clientId));

            var menuItems = menuItemsEnumerable as Tuple<int, int>[] ?? menuItemsEnumerable.ToArray();
            if (menuItems == null || !menuItems.Any())
                throw new ArgumentNullException("menuItemsEnumerable");

            OrderEntity orderEntity = null;

            using (var db = new DataAccessProvider())
            {
                UserContextEntity client = db.Users.Find(clientId);
                if(client == null)
                    throw new ArgumentException(String.Format("No such user (id={0}) exists.", clientId));
                if(client.Role != UserRole.Client)
                    throw new ArgumentException(String.Format("User (id={0}) is not a client.", clientId));

                TableEntity table = db.Tables.Find(tableId);
                if (table == null)
                    throw new ArgumentException(String.Format("No such table (id={0}) exists.", tableId));

                orderEntity = new OrderEntity() { Client = client, Table = table, State = OrderState.Placed, PlacingDate = DateTime.Now, ClosingDate = DateTime.MaxValue };

                foreach (var tuple in menuItems)
                {
                    MenuItemEntity menuItem = db.MenuItems.Find(tuple.Item1);
                    if (menuItem == null)
                        throw new ArgumentException(String.Format("No such menuItem (id={0}) exists", tuple.Item1));
                    if (tuple.Item2 <= 0)
                        throw new ArgumentException(String.Format("MenuItem id={0} has quantity={1}", tuple.Item1, tuple.Item2));

                    MenuItemQuantityEntity menuItemQuantity = new MenuItemQuantityEntity() { MenuItem = menuItem, Quantity = tuple.Item2 };
                    orderEntity.MenuItems.Add(menuItemQuantity);
                }

                orderEntity = db.Orders.Add(orderEntity);
                db.SaveChanges();

                var order =  new Order(orderEntity);
                logger.Write(String.Format("Order {0} by user {1} was created.", order.Id, order.Client), LoggingCategory.Information);

                Task.Run(() => AssignOrder(order));

                return order;
            }
        }

        /// <summary>
        /// Dodawanie zamówienia z poziomu strony internetowej. Od zamówienia z lokalu różni się tym, iż klient podaje datę, kiedy zamierza przyjść do lokalu.
        /// Wybór stolika następuje automatycznie
        /// </summary>
        /// <param name="userId">Identyfikator klienta</param>
        /// <param name="orderTime">Czas obsługi zamówienia</param>
        /// <param name="menuItemsEnumerable">Elementy stanowiące zamówienie</param>
        /// <returns></returns>
        public Order AddOrder(int clientId, DateTime orderTime, IEnumerable<Tuple<int, int>> menuItemsEnumerable)
        {
            if (!CheckHasUserRole(clientId, UserRole.Client))
                throw new SecurityException(String.Format("Client id={0} is not logged in.", clientId));

            // Data zamówienia jest w przeszłości
            if (orderTime.CompareTo(DateTime.Now) < 0)
                throw new ArgumentException("Specified date already passed.", "orderTime");

            var menuItems = menuItemsEnumerable as Tuple<int, int>[] ?? menuItemsEnumerable.ToArray();
            if (menuItems == null || !menuItems.Any())
                throw new ArgumentNullException("menuItemsEnumerable");

            OrderEntity orderEntity = null;

            using (var db = new DataAccessProvider())
            {
                UserContextEntity client = db.Users.Find(clientId);
                if (client == null)
                    throw new ArgumentException(String.Format("No such user (id={0}) exists.", clientId));
                if (client.Role != UserRole.Client)
                    throw new ArgumentException(String.Format("User (id={0}) is not a client.", clientId));

                orderEntity = new OrderEntity() { Client = client, State = OrderState.Placed, PlacingDate = DateTime.Now, ClosingDate = DateTime.MaxValue };

                foreach (var tuple in menuItems)
                {
                    MenuItemEntity menuItem = db.MenuItems.Find(tuple.Item1);
                    if (menuItem == null)
                        throw new ArgumentException(String.Format("No such menuItem (id={0}) exists", tuple.Item1));
                    if (tuple.Item2 <= 0)
                        throw new ArgumentException(String.Format("MenuItem id={0} has quantity={1}", tuple.Item1, tuple.Item2));

                    MenuItemQuantityEntity menuItemQuantity = new MenuItemQuantityEntity() { MenuItem = menuItem, Quantity = tuple.Item2 };
                    orderEntity.MenuItems.Add(menuItemQuantity);
                }

                orderEntity = db.Orders.Add(orderEntity);
                db.SaveChanges();

                var order = new Order(orderEntity);

                orderAssignScheduler.AddOrder(new OrderServicingDateWrapper(order, orderTime));
                logger.Write(String.Format("Order {0} by user {1} added to scheduler. Deploy date {2}", order.Id, order.Client, orderTime), LoggingCategory.Information);
                return order;
            }
            
        }

        public bool PayForOrder(int userId, int orderId)
        {
            if(!CheckHasUserRole(userId, UserRole.Client))
                throw new SecurityException(String.Format("Client id={0} is not logged in.", userId));

            return PayForOrderInternal(userId, orderId);
        }

        public IEnumerable<Order> GetOrders(int clientId)
        {
            if (!CheckHasUserRole(clientId, UserRole.Client))
                throw new SecurityException(String.Format("Client id={0} is not logged in.", clientId));

            using (var db = new DataAccessProvider())
            {
                var orderList = db.Orders.Include("Waiter").Include("Table").Include("MenuItems").Include("Client").Where(o => !o.IsDeleted && o.Client.Id == clientId).ToList();
                return orderList.Select(o => new Order(o)).ToList();
            }
        }
        #endregion

        #region Private Methods

        private bool CheckIsUserLoggedIn(int userId)
        {
            lock(loggedInUsersLockObject)
                return loggedInUsers.FirstOrDefault(c => c.Id.Equals(userId)) != null;
        }

        private bool CheckHasUserRole(int userId, UserRole role)
        {
            lock (loggedInUsersLockObject)
            {
                UserContextEntity user = loggedInUsers.FirstOrDefault(c => c.Id.Equals(userId));
                if (user == null)
                    return false;
                return (user.Role & role) != 0;
            }
        }

        private bool RemoveFromLoggedInUsers(int userId)
        {
            lock (loggedInUsersLockObject)
            {
                lock (clientRegistrationRecordsLockObject)
                {
                    lock (waiterOrderDictionaryLockObject)
                    {
                        UserContextEntity userToRemove = this.loggedInUsers.FirstOrDefault(c => c.Id.Equals(userId));
                        if (userToRemove == null)
                            return false;

                        loggedInUsers.Remove(userToRemove);

                        if (userToRemove.Role == UserRole.Client)
                            clientRegistrationRecords.RemoveWhere(r => r.ClientId == userId);
                        else if (userToRemove.Role == UserRole.Waiter)
                        {
                            var waiterRegRec = waiterOrderDictionary.Keys.FirstOrDefault(r => r.WaiterId == userId);
                            if (waiterRegRec != null)
                                waiterOrderDictionary.Remove(waiterRegRec);
                        }

                        logger.Write(String.Format("User {0} ({1}) logged out", userToRemove.Login, userToRemove.Role), LoggingCategory.Information);
                        return true;
                    }
                }
            }
           
        }

        private UserContext AddUserToDatabase(string firstName, string lastName, string login, string password, UserRole role)
        {
            if (String.IsNullOrEmpty(firstName))
                throw new ArgumentNullException("firstName is null");
            if (String.IsNullOrEmpty(lastName))
                throw new ArgumentNullException("lastName is null");
            if (String.IsNullOrEmpty(login))
                throw new ArgumentNullException("login is null");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password is null");

            UserContextEntity newUser = null;

            using (var db = new DataAccessProvider())
            {
                //sprawdzenie czy dany login już istnieje
                var userSameLogin = db.Users.FirstOrDefault(u => u.Role == role && u.Login == login);

                if (userSameLogin != null)
                    throw new ArgumentException(String.Format("There already is an user with login = {0}.", login));

                newUser = new UserContextEntity()
                {
                    Login = login,
                    FirstName = firstName,
                    LastName = lastName,
                    Role = role
                };

                newUser = db.Users.Add(newUser);
                db.SaveChanges();

                var userPassword = new PasswordEntity() { UserId = newUser.Id, Hash = HashClass.CreateSecondHash(password) };
                db.Passwords.Add(userPassword);
                db.SaveChanges();
            }

            return new UserContext(newUser);
        }

        private UserContext SetWaiterForOrder(int orderId, int waiterId)
        {
            using (var db = new DataAccessProvider())
            {
                var waiter = db.Users.Find(waiterId);
                if (waiter == null)
                    return null;

                var order = db.Orders.Find(orderId);
                if (order == null)
                    return null;

                if (waiter.Role != UserRole.Waiter)
                    return null;

                order.Waiter = waiter;
                db.SaveChanges();
                return new UserContext(waiter);
            }
        }
        #endregion

        #region OrderLogic
        /// <summary>
        /// Przyporządkowuje zadanie kelnerom
        /// </summary>
        /// <param name="orderToAssign">Zadanie do przyporządkowania</param>
        /// <param name="waiterToAssign">Jeżeli nie jest nullem, to metoda spróbuje zadanie przyporządkować (tylko) temu kelnerowi</param>
        private void AssignOrder(Order orderToAssign, UserContext waiterToAssign = null)
        {
            if (orderToAssign == null)
                return;

            if (waiterToAssign != null && waiterToAssign.Role != UserRole.Waiter)
            {
                string errorMessage = String.Format("Trying to assign order to non-waiter, id={0}", waiterToAssign.Id);
                logger.Write(errorMessage, LoggingCategory.Warning);
                throw new ArgumentException(errorMessage, "waiterToAssign");
            }

            bool foundWaiter = false;

            ClientRegistrationRecord clientRegistrationRecord = null;

            lock(clientRegistrationRecordsLockObject)
                clientRegistrationRecord = clientRegistrationRecords.FirstOrDefault(r => r.ClientId == orderToAssign.Client.Id);

            CheckAreWaitersAvailable();


            lock (waiterOrderDictionary)
            {
                if (waiterToAssign != null)
                {
                    var waiterRegistrationRecord =
                        waiterOrderDictionary.Keys.FirstOrDefault(r => r.WaiterId == waiterToAssign.Id);
                    if(waiterRegistrationRecord != null && waiterOrderDictionary[waiterRegistrationRecord] == null)
                        if (waiterRegistrationRecord.Callback.AcceptNewOrder(orderToAssign))
                        {
                            foundWaiter = true;
                            AcceptOrderInternal(waiterRegistrationRecord, clientRegistrationRecord, orderToAssign);
                        }
                }
                else
                {
                    foreach (var registrationRecord in waiterOrderDictionary.Keys)
                    {
                        if (waiterOrderDictionary[registrationRecord] == null)
                        {
                            if (registrationRecord.Callback.AcceptNewOrder(orderToAssign))
                            {
                                foundWaiter = true;
                                AcceptOrderInternal(registrationRecord, clientRegistrationRecord, orderToAssign);
                                break;
                            }
                        }
                    }

                }
            }
           

            if (!foundWaiter)
            {
                //nie udało się przydzielić zamówienia żadnemu kelnerowi
                lock(awaitingOrderCollectionLockObject)
                    awaitingOrderCollection.Enqueue(orderToAssign);
                logger.Write(String.Format("Order {0} was added to awaitingOrderCollection", orderToAssign.Id), LoggingCategory.Information);
                //powiadamiamy klienta o braku dostępnych kelnerów
                if(clientRegistrationRecord != null)
                    Task.Run(() => clientRegistrationRecord.Callback.NotifyOrderOnHold(orderToAssign.Id));
            }
        }

        /// <summary>
        /// Zapisuje w bazie danych odpowiedni stan zamówienia oraz powiadamia klienta o tym, że jego zamówienie zostało przyjęte.
        /// </summary>
        /// <param name="waiterRegistrationRecord">Kelner, który przyjął zamówienie</param>
        /// <param name="clientRegistrationRecord">Klient składający zamówienie</param>
        /// <param name="orderToAssign">Zamówienie</param>
        private void AcceptOrderInternal(WaiterRegistrationRecord waiterRegistrationRecord, ClientRegistrationRecord clientRegistrationRecord, Order  orderToAssign)
        {
            //zapisujemy, że kelner będzie się zajmował tym zadaniem
            waiterOrderDictionary[waiterRegistrationRecord] = orderToAssign;
            //ustawiamy kelnera jako obsługujący zadanie w bazie danych
            var waiterContext = SetWaiterForOrder(orderToAssign.Id, waiterRegistrationRecord.WaiterId);
            //stan zamówienia ustawiamy jako zaakceptowany
            SetOrderState(waiterRegistrationRecord.WaiterId, orderToAssign.Id, OrderState.Accepted);
            logger.Write(String.Format("Order {0} was accepted by waiter {1}({2})", orderToAssign.Id, waiterContext.Login, waiterRegistrationRecord.WaiterId), LoggingCategory.Information);
            //powiadamiamy klienta o zaakceptowaniu zamówienia (jeżeli istnieje)
            if (clientRegistrationRecord != null)
            {
                clientRegistrationRecord.Callback.NotifyOrderAccepted(orderToAssign.Id, waiterContext);

                lock (loggedInUsersLockObject)
                {
                    var clientContext =
                        loggedInUsers.FirstOrDefault(u => u.Id == clientRegistrationRecord.ClientId);

                    if (clientContext != null)
                        logger.Write(
                            String.Format(
                                "User {0}({1}) was notified of order {2} being accepted by waiter {3}({4})",
                                clientContext.Login, clientContext.Id, orderToAssign.Id,
                                waiterContext.Login, waiterRegistrationRecord.WaiterId),
                            LoggingCategory.Information);
                }
            }
        }

        /// <summary>
        /// Próbuję przypasować kelnera do oczekującego zadania
        /// </summary>
        /// <param name="waiterToAssign">Kelner, któremu ma zostać przypisane zadanie.</param>
        /// <remarks>Jeżeli <href>waiterToAssign</href> jest równe null, to metoda leci po wszystkich wolnych kelnerach</remarks>
        private void TryAssignAwaitingOrder(UserContext waiterToAssign = null)
        {
            Order order = null;
            lock (awaitingOrderCollectionLockObject)
            {
                if (awaitingOrderCollection.Count == 0)
                    return;

                order = awaitingOrderCollection.Dequeue();
            }
            if(order != null)
                AssignOrder(order, waiterToAssign);
        }

        /// <summary>
        /// Sprawdza stan kanału komunikacyjnego.
        /// </summary>
        /// <param name="communicationObject">Kanał komunikacyjny</param>
        /// <returns>Czy kanał jest gotowy do wysyłania wiadomości</returns>
        private bool IsAvailable(ICommunicationObject communicationObject)
        {
            return communicationObject.State == CommunicationState.Opened;
        }

        /// <summary>
        /// Oczyszcza kolekcję zalogowanych kelnerów z tych, których kanały komunikacyjne nie są otwarte.
        /// </summary>
        private void CheckAreWaitersAvailable()
        {
            IList<WaiterRegistrationRecord> notLongerAvailableWaiters = new List<WaiterRegistrationRecord>();

            lock(waiterOrderDictionary)
                foreach (var waiterRegistrationRecord in waiterOrderDictionary.Keys)
                    if(!IsAvailable(waiterRegistrationRecord.Callback as ICommunicationObject))
                        notLongerAvailableWaiters.Add(waiterRegistrationRecord);

            foreach (var waiterRegistrationRecord in notLongerAvailableWaiters)
            {
                RemoveFromLoggedInUsers(waiterRegistrationRecord.WaiterId);
                logger.Write(String.Format("Waiter {0} is no longer available.", waiterRegistrationRecord.WaiterId),
                    LoggingCategory.Warning);
            }
        }

        /// <summary>
        /// Oczyszcza kolekcję zalogowanych klientów z tych, których kanały komunikacyjne nie są otwarte.
        /// </summary>
        private void CheckAreClientsAvailable()
        {
            IList<ClientRegistrationRecord> notLongerAvailableClients = new List<ClientRegistrationRecord>();

            lock(clientRegistrationRecordsLockObject)
                foreach(var clientRegistrationRecord in clientRegistrationRecords)
                    if(!IsAvailable(clientRegistrationRecord.Callback as ICommunicationObject))
                        notLongerAvailableClients.Add(clientRegistrationRecord);

            foreach (var clientRegistrationRecord in notLongerAvailableClients)
            {
                RemoveFromLoggedInUsers(clientRegistrationRecord.ClientId);
                logger.Write(String.Format("Client {0} is no longer available.", clientRegistrationRecord.ClientId),
                    LoggingCategory.Warning);
            }
        }

        /// <summary>
        /// Szuka wolnego stolika i próbuje dopasować kelnera do zadania
        /// </summary>
        /// <param name="orderToAssign">Zadanie do przyporządkowania</param>
        /// <remarks>Szukanie stolika odbywa się poprzez usuwanie z kolekcji wszystkich stolików tych, dla których aktualnie istnieją aktywne zamówienia</remarks>
        /// <returns></returns>
        private bool FindTableAndAssignOrder(Order orderToAssign)
        {
            if(orderToAssign == null)
                throw new ArgumentNullException("orderToAssign");

            using (var db = new DataAccessProvider())
            {
                //znalezienie wolnego stolika
                var orderEntity = db.Orders.Include("MenuItems").Include("MenuItems.MenuItem").Include("MenuItems.MenuItem.Category").Include("Waiter").Include("Table").Include("Client").FirstOrDefault(o => o.Id == orderToAssign.Id);
                if (orderEntity == null)
                    throw new ArgumentException(String.Format("No order of id = {0} exists.", orderToAssign.Id));

                var tableList = db.Tables.ToList();

                //Nie ma w bazie stolików, nie można przydzielić zamówienia
                if (tableList == null || !tableList.Any())
                    return false;

                var occupiedTableList =
                    db.Orders.Include("Table")
                        .Where(o => o.State == OrderState.Accepted || o.State == OrderState.AwaitingDelivery)
                        .Select(o => o.Table)
                        .ToList();

                if(occupiedTableList != null && occupiedTableList.Any())
                    foreach (var table in occupiedTableList)
                        tableList.Remove(table);

                //wszystkie stoliki są zajęte
                if (tableList.Count == 0)
                {
                    logger.Write(String.Format("There is no free table for order {0} by client {1}({2})", orderToAssign.Id, orderToAssign.Client.Login, orderToAssign.Client.Id), LoggingCategory.Warning);
                    return false;
                }

                orderEntity.Table = tableList.First();
                db.Entry(orderEntity).State = EntityState.Modified;
                orderToAssign = new Order(orderEntity);
                db.SaveChanges();

                Task.Run(() => AssignOrder(orderToAssign));
                return true;
            }
        }

        /// <summary>
        /// Wysyła do kelnera zapytanie o potwierdzenie dokonania płatności przez klienta. Jeżeli potwierdzenie zostanie uzyskane, w bazie jest zmieniany stan zamówienia.
        /// </summary>
        /// <param name="userId">Identyfikator klienta</param>
        /// <param name="orderId">Identyfikator zamówienia</param>
        /// <returns></returns>
        private bool PayForOrderInternal(int userId, int orderId)
        {
            WaiterRegistrationRecord waiterRegRecord = null;

            lock(waiterOrderDictionary)
                foreach(var pair in waiterOrderDictionary)
                    if (pair.Value != null && pair.Value.Id == orderId)
                        waiterRegRecord = pair.Key;

            //Zadanie nie jest aktualnie w realizacji
            if (waiterRegRecord == null)
                return false;

            //Kelner potwierdził zapłatę
            if (waiterRegRecord.Callback.ConfirmUserPaid(userId))
            {
                //Ustawiamy stan na zrealizowany
                SetOrderState(waiterRegRecord.WaiterId, orderId, OrderState.Realized);
                //Kelner jest wolny, gotowy na następne zamówienie
                lock(waiterOrderDictionary)
                    waiterOrderDictionary[waiterRegRecord] = null;

                logger.Write(String.Format("Order {0} by user {1} was paid for.", orderId, userId), LoggingCategory.Information);

                //Jeżeli są zadania oczekujące, próbujemy je przydzielić
                Task.Run(() => TryAssignAwaitingOrder());

                return true;
            }

            //Kelner nie potwierdził zapłatę zamówienia
            return false;
        }
        #endregion

        #region IDataWipe

        bool IDataWipe.WipeMenuItemCategory(int categoryId)
        {
            using (var db = new DataAccessProvider())
            {
                MenuItemCategoryEntity menuItemCategoryToRemove = db.MenuItemCategories.Find(categoryId);
                if (menuItemCategoryToRemove == null)
                    return false;

                db.MenuItemCategories.Remove(menuItemCategoryToRemove);
                db.SaveChanges();
                return true;
            }
        }

        bool IDataWipe.WipeMenuItem(int menuItemId)
        {
            using (var db = new DataAccessProvider())
            {
                MenuItemEntity menuItemToRemove = db.MenuItems.Find(menuItemId);
                if (menuItemToRemove == null)
                    return false;

                var menuItemQuantityEntitiesToRemove = db.MenuItemQuantities.Where(q => q.MenuItem.Id == menuItemId);
                foreach(MenuItemQuantityEntity menuItemQuantityEntity in menuItemQuantityEntitiesToRemove)
                    db.MenuItemQuantities.Remove(menuItemQuantityEntity);

                db.MenuItems.Remove(menuItemToRemove);
                db.SaveChanges();
                return true;
            }        
        }

        bool IDataWipe.WipeUser(int userId)
        {
            using (var db = new DataAccessProvider())
            {
                UserContextEntity userContextToRemove = db.Users.Find(userId);
                if (userContextToRemove == null)
                    return false;

                if (CheckIsUserLoggedIn(userContextToRemove.Id))
                    RemoveFromLoggedInUsers(userContextToRemove.Id);

                db.Users.Remove(userContextToRemove);

                PasswordEntity passwordToRemove = db.Passwords.FirstOrDefault(p => p.UserId == userId);
                if (passwordToRemove == null)
                    return false;

                db.Passwords.Remove(passwordToRemove);

                db.SaveChanges();
                return true;
            }
        }

        bool IDataWipe.WipeTable(int tableId)
        {
            using (var db = new DataAccessProvider())
            {
                TableEntity tableToRemove = db.Tables.Find(tableId);
                if (tableToRemove == null)
                    return false;

                db.Tables.Remove(tableToRemove);
                db.SaveChanges();
                return true;
            }
        }

        bool IDataWipe.WipeOrder(int orderId)
        {
            using(var db = new DataAccessProvider())
            {
                OrderEntity orderToRemove = db.Orders.Find(orderId);
                if (orderToRemove == null)
                    return false;

                var quantityList = orderToRemove.MenuItems.ToList();
                foreach (MenuItemQuantityEntity quantity in quantityList)
                    db.MenuItemQuantities.Remove(quantity);

                db.Orders.Remove(orderToRemove);
                db.SaveChanges();
                return true;
            }
        }

        #endregion
    }
}
