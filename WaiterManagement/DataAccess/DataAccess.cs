using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLib.DbDataStructures;
using System.Security;
using System.Data.Entity;
using DataAccess.Migrations;
using ClassLib.DataStructures;
using ClassLib.ServiceContracts;

namespace DataAccess
{
    /// <summary>
    /// Klasa agregująca metody dostępu do bazy danych
    /// </summary>
    public class DataAccessClass : IManagerDataAccessWCFService, IWaiterDataAccessWCFService, IDataWipe, IManagerDataAccess, IWaiterDataAccess
    {
        #region Private Fields
        private HashSet<UserContext> loggedInUsers;
        #endregion

        #region Constructors
        public DataAccessClass()
        {
            loggedInUsers = new HashSet<UserContext>();
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataAccessProvider, Configuration>()); 
        }
        #endregion

        #region IBaseDataAccess
        public IEnumerable<MenuItemCategory> GetMenuItemCategories(int userId)
        {
            if (CheckIsUserLoggedIn(userId))
                throw new SecurityException(String.Format("User login={0} is not logged in", userId));

            using (var db = new DataAccessProvider())
            {
                var menuItemCategoryList = db.MenuItemCategories.Where( m => !m.IsDeleted).ToList();
                return menuItemCategoryList;
            }
        }

        public IEnumerable<MenuItem> GetMenuItems(int userId)
        {
            if (CheckIsUserLoggedIn(userId))
                throw new SecurityException(String.Format("User login={0} is not logged in", userId));

            using (var db = new DataAccessProvider())
            {
                var menuItemList = db.MenuItems.Include("Category").Where( mI=> !mI.IsDeleted).ToList();
                return menuItemList;
            }
        }

        public IEnumerable<Table> GetTables(int userId)
        {
            if (CheckIsUserLoggedIn(userId))
                throw new SecurityException(String.Format("User login={0} is not logged in", userId));

            using (var db = new DataAccessProvider())
            {
                var tableList = db.Tables.Where( t => !t.IsDeleted).ToList();
                return tableList;
            }
        }

        public UserContext LogIn(string login, string password)
        {
            if (String.IsNullOrEmpty(login))
                throw new ArgumentNullException("login is null");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password is null");

            UserContext userContext = null;

            using (var db = new DataAccessProvider())
            {
                userContext = db.Users.FirstOrDefault(u => u.Login.Equals(login));
                if (userContext == null)
                    return null;

                string userHash = db.Passwords.Find(userContext.Id).Hash;
                if (String.IsNullOrEmpty(userHash))
                    return null;

                if(!HashClass.ValidatePassword(password, userHash))
                    return null;
            }

            if (userContext != null)
            {
                if (CheckIsUserLoggedIn(userContext.Id))
                    throw new SecurityException(String.Format("User login={0} is already logged in", userContext.Login));
                loggedInUsers.Add(userContext);
            }

            return userContext;
        }

        public bool LogOut(int userId)
        {
            if (!CheckIsUserLoggedIn(userId))
                return false;

            return RemoveFromLoggedInUsers(userId);
        }

        #endregion

        #region IManagerDataAccess
        public UserContext AddManager(string firstName, string lastName, string login, string password)
        {
            if (String.IsNullOrEmpty(firstName))
                throw new ArgumentNullException("firstName is null");
            if (String.IsNullOrEmpty(lastName))
                throw new ArgumentNullException("lastName is null");
            if (String.IsNullOrEmpty(login))
                throw new ArgumentNullException("login is null");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password is null");

            UserContext newManager = null;

            using(var db = new DataAccessProvider())
            {
                //sprawdzenie czy dany login już istnieje
                var managerSameLogin = db.Users.FirstOrDefault(u => u.Role == UserRole.Manager && u.Login == login);

                if(managerSameLogin != null)
                    throw new ArgumentException(String.Format("There already is a manager with login = {0}.", login));

                newManager = new UserContext() { Login = login, FirstName = firstName, LastName = lastName, Role = UserRole.Manager };

                newManager = db.Users.Add(newManager);
                db.SaveChanges();
            }

            return newManager;
        }

        public MenuItemCategory AddMenuItemCategory(int managerId, string name, string description)
        {
            if (!CheckHasUserRole(managerId, UserRole.Manager))
                throw new SecurityException(String.Format("User id = {0} is not logged in or is no manager", managerId));

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("Name is null");
            if (String.IsNullOrEmpty(description))
                throw new ArgumentNullException("Description is null");           

            MenuItemCategory newCategory = null;

            using( var db = new DataAccessProvider())
            {
                var categoryToAdd = new MenuItemCategory() {Name = name, Description = description};

                var categoriesSameName = db.MenuItemCategories.Where(c => c.Name.Equals(name));
                
                if(categoriesSameName != null && categoriesSameName.Any())
                    foreach(MenuItemCategory category in categoriesSameName)
                        if(category.Equals(categoryToAdd))
                        {
                            if(category.IsDeleted)
                                category.IsDeleted = false;
                            newCategory = category;
                            break;
                        }

                if(newCategory == null)
                    newCategory = db.MenuItemCategories.Add(categoryToAdd);

                db.SaveChanges();
            }

            return newCategory;
        }

        public bool EditMenuItemCategory(int managerId, MenuItemCategory menuItemCategoryToEdit)
        {
            if (!CheckHasUserRole(managerId, UserRole.Manager))
                throw new SecurityException(String.Format("User id = {0} is not logged in or is no manager", managerId));

            if (menuItemCategoryToEdit == null)
                throw new ArgumentNullException("menuItemCategoryToEdit is null");           

            using(var db = new DataAccessProvider())
            {
                MenuItemCategory editedMenuItemCategory = db.MenuItemCategories.Find(menuItemCategoryToEdit.Id);
                if (editedMenuItemCategory == null || editedMenuItemCategory.IsDeleted)
                    return false;
                db.Entry(editedMenuItemCategory).State = System.Data.Entity.EntityState.Detached;
                db.MenuItemCategories.Attach(menuItemCategoryToEdit);
                db.Entry(menuItemCategoryToEdit).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return true;
            }
        }

        public bool RemoveMenuItemCategory(int managerId, int categoryId)
        {
            if (!CheckHasUserRole(managerId, UserRole.Manager))
                throw new SecurityException(String.Format("User id = {0} is not logged in or is no manager", managerId));

            using(var db = new DataAccessProvider())
            {
                MenuItemCategory menuItemCategoryToRemove = db.MenuItemCategories.Find(categoryId);
                if (menuItemCategoryToRemove == null || menuItemCategoryToRemove.IsDeleted)
                    return false;
                
                //db.MenuItemCategories.Remove(menuItemCategoryToRemove);
                menuItemCategoryToRemove.IsDeleted = true;
                db.SaveChanges();
                return true;
            }
        }

        public MenuItem AddMenuItem(int managerId, string name, string description, int categoryId, Money price)
        {
            if (!CheckHasUserRole(managerId, UserRole.Manager))
                throw new SecurityException(String.Format("User id = {0} is not logged in or is no manager", managerId));

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("name is null");
            if (String.IsNullOrEmpty(description))
                throw new ArgumentNullException("description is null");

            MenuItem newMenuItem = null;
            MenuItemCategory category = null;

            using (var db = new DataAccessProvider())
            {
                category = db.MenuItemCategories.Find(categoryId);
                if (category == null)
                    return null;

                var menuItemToAdd = new MenuItem() { Name = name, Description = description, Category = category, Price = price };

                var menuItemsSameName = db.MenuItems.Where(mI => mI.Name.Equals(name));
                if(menuItemsSameName != null && menuItemsSameName.Any())
                    foreach(MenuItem menuItem in menuItemsSameName)
                        if(menuItem.Equals(menuItemToAdd))
                        {
                            if (menuItem.IsDeleted)
                                menuItem.IsDeleted = false;

                            newMenuItem = menuItem;
                            break;
                        }
                if(newMenuItem == null)
                    newMenuItem = db.MenuItems.Add(menuItemToAdd);

                db.SaveChanges();
            }

            return newMenuItem;
        }

        public bool EditMenuItem(int managerId, MenuItem menuItemToEdit)
        {
            if (!CheckHasUserRole(managerId, UserRole.Manager))
                throw new SecurityException(String.Format("User id = {0} is not logged in or is no manager", managerId));

            if (menuItemToEdit == null)
                throw new ArgumentNullException("menuItemToEdit is null");
            
            using(var db = new DataAccessProvider())
            {
                MenuItem editedMenuItem = db.MenuItems.Find(menuItemToEdit.Id);
                if (editedMenuItem == null || editedMenuItem.IsDeleted)
                    return false;
                db.Entry(editedMenuItem).State = System.Data.Entity.EntityState.Detached;
                db.MenuItems.Attach(menuItemToEdit);
                db.Entry(menuItemToEdit).State = System.Data.Entity.EntityState.Modified;
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
                MenuItem menuItemToRemove = db.MenuItems.Find(menuItemId);
                if (menuItemToRemove == null || menuItemToRemove.IsDeleted)
                    return false;
                
                //db.MenuItems.Remove(menuItemToRemove);
                menuItemToRemove.IsDeleted = true;
                db.SaveChanges();
                return true;
            }            
        }        

        public UserContext AddWaiter(int managerId, string firstName, string lastName, string login, string password)
        {
            if (!CheckHasUserRole(managerId, UserRole.Manager))
                throw new SecurityException(String.Format("User id = {0} is not logged in or is no manager", managerId));

            if (String.IsNullOrEmpty(firstName))
                throw new ArgumentNullException("firstName is null");
            if (String.IsNullOrEmpty(lastName))
                throw new ArgumentNullException("lastName is null");
            if (String.IsNullOrEmpty(login))
                throw new ArgumentNullException("login is null");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password is null");

            UserContext newWaiterContext = null;

            //TODO: filtrować po kelnerach tylko
            //TODO: zapisać hasło
            using (var db = new DataAccessProvider())
            {
                var waiterContextToAdd = new UserContext() { FirstName = firstName, LastName = lastName, Login = login, Role = UserRole.Waiter};
                var usersSameLogin = db.Users.Where(u => u.Login.Equals(login));

                if (usersSameLogin != null && usersSameLogin.Any())
                {
                    foreach (UserContext userContext in usersSameLogin)
                        if (userContext.Equals(waiterContextToAdd))
                        {
                            if (userContext.IsDeleted)
                                userContext.IsDeleted = false;

                            newWaiterContext = userContext;
                            break;
                        }

                    //istnieją kelnerzy o tym samym loginie, ale nie są tacy sami jak ten co chcemy dodać.
                    if(newWaiterContext == null)
                        throw new ArgumentException(String.Format("login = {0} already exists in database!", login));
                }                    

                if(newWaiterContext == null)
                    newWaiterContext = db.Users.Add(waiterContextToAdd);

                db.SaveChanges();
            }

            return newWaiterContext;
        }

        public bool EditWaiter(int managerId, UserContext waiterToEdit)
        {
            if (!CheckHasUserRole(managerId, UserRole.Manager))
                throw new SecurityException(String.Format("User id = {0} is not logged in or is no manager", managerId));

            if (waiterToEdit == null)
                throw new ArgumentNullException("waiterToEdit is null");

            if (waiterToEdit.Role != UserRole.Waiter)
                throw new ArgumentException(String.Format("User id = {0} is no waiter.", waiterToEdit.Id));

            using(var db = new DataAccessProvider())
            {
                UserContext editedWaiterContext = db.Users.Find(waiterToEdit.Id);
                if (editedWaiterContext == null || editedWaiterContext.IsDeleted)
                    return false;

                db.Entry(editedWaiterContext).State = System.Data.Entity.EntityState.Detached;
                db.Users.Attach(waiterToEdit);
                db.Entry(waiterToEdit).State = System.Data.Entity.EntityState.Modified;
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
                UserContext waiterContextToRemove = db.Users.Find(waiterId);
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
                return waiterList;
            }
        }

        public Table AddTable(int managerId, int tableNumber, string description)
        {
            if (!CheckHasUserRole(managerId, UserRole.Manager))
                throw new SecurityException(String.Format("User id = {0} is not logged in or is no manager", managerId));

            if (String.IsNullOrEmpty(description))
                throw new ArgumentNullException("description is null");

            Table newTable = null;

            using (var db = new DataAccessProvider())
            {
                var tableToAdd = new Table() { Number = tableNumber, Description = description };

                var tablesSameNumber = db.Tables.Where(t => t.Number.Equals(tableNumber));
                if(tablesSameNumber != null && tablesSameNumber.Any())
                    foreach(Table table in tablesSameNumber)
                        if(table.Equals(tableToAdd))
                        {
                            if (table.IsDeleted)
                                table.IsDeleted = false;

                            newTable = table;
                            break;
                        }

                if(newTable == null)
                    newTable = db.Tables.Add(tableToAdd);

                db.SaveChanges();
            }

            return newTable;
        }

        public bool EditTable(int managerId, Table tableToEdit)
        {
            if (!CheckHasUserRole(managerId, UserRole.Manager))
                throw new SecurityException(String.Format("User id = {0} is not logged in or is no manager", managerId));

            if (tableToEdit == null)
                throw new ArgumentNullException("tableToEdit is null");

            using(var db = new DataAccessProvider())
            {
                Table editedTable = db.Tables.Find(tableToEdit.Id);
                if (editedTable == null || editedTable.IsDeleted)
                    return false;
                db.Entry(editedTable).State = System.Data.Entity.EntityState.Detached;
                db.Tables.Attach(tableToEdit);
                db.Entry(tableToEdit).State = System.Data.Entity.EntityState.Modified;
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
                Table tableToRemove = db.Tables.Find(tableId);
                if(tableToRemove == null || tableToRemove.IsDeleted)
                    return false;

                //db.Tables.Remove(tableToRemove);
                tableToRemove.IsDeleted = true;
                db.SaveChanges();
                return true;
            }
        }        

        public IEnumerable<Order> GetOrders(int managerId)
        {
            if (!CheckHasUserRole(managerId, UserRole.Manager))
                throw new SecurityException(String.Format("User id = {0} is not logged in or is no manager", managerId));

            using(var db = new DataAccessProvider())
            {
                var orderList = db.Orders.Include("Waiter").Include("Table").Include("MenuItems").Where(o => !o.IsDeleted).ToList();
                return orderList;
            }
        }

        public bool RemoveOrder(int managerId, int orderId)
        {
            if (!CheckHasUserRole(managerId, UserRole.Manager))
                throw new SecurityException(String.Format("User id = {0} is not logged in or is no manager", managerId));

            using(var db = new DataAccessProvider())
            {
                Order orderToRemove = db.Orders.Find(orderId);
                if (orderToRemove == null || orderToRemove.IsDeleted)
                    return false;

                var quantityList = orderToRemove.MenuItems.ToList();
                foreach (MenuItemQuantity quantity in quantityList)
                    //db.MenuItemQuantities.Remove(quantity);
                    quantity.IsDeleted = true;

                //db.Orders.Remove(orderToRemove);
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
                return orders;
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
                    return null;

                //Indeks końcowy i początkowy taki sam - zwracamy jedną wartość
                if (firstIndex == lastIndex)
                    return new Order[1] { sortedList[firstIndex] };

                //Kelner ma mniej zamówień niż indeks końcowy - zwracamy od indeksu początkowego do końca
                if(sortedList.Count < lastIndex + 1)
                {
                    var result = new Order[sortedList.Count - firstIndex];
                    sortedList.CopyTo(firstIndex, result, 0, sortedList.Count - firstIndex);
                    return result;
                }

                var resultOrders = new Order[lastIndex - firstIndex + 1];
                sortedList.CopyTo(firstIndex, resultOrders, 0, lastIndex - firstIndex + 1);
                return resultOrders;                
            }
        }

        public IEnumerable<Order> GetActiveOrders(int waiterId)
        {
            if (!CheckHasUserRole(waiterId, UserRole.Waiter))
                throw new SecurityException(String.Format("User id={0} is not logged in or is not a waiter.", waiterId));

            using(var db = new DataAccessProvider())
            {
                var activeOrders = db.Orders.Include("MenuItems").Include("MenuItems.MenuItem").Include("MenuItems.MenuItem.Category").Include("Waiter").Include("Table").Where( o => o.Waiter.Id == waiterId && !o.IsDeleted && (o.State == OrderState.Accepted || o.State == OrderState.Placed )).ToList();
                
                return activeOrders;
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
                var order = db.Orders.Find(orderId);
                if (order == null)
                    throw new ArgumentException(String.Format("No such Order (id={0}) exists", orderId));

               
                if (state.Equals(OrderState.Accepted))
                {
                    //Nie można zaakceptować zamówienia, który jest w stanie innym niż Placed
                    if(!order.State.Equals(OrderState.Placed))
                        return false;
                }
                else if(state.Equals(OrderState.Realized) || state.Equals(OrderState.NotRealized) )
                {
                    //Nie można zmienić stan zamówienia innego kelnera
                    if (order.Waiter.Id != waiterId)
                        return false;

                    //Nie można zakończyć zamówienie, które nie jest w stanie accepted
                    if (!order.State.Equals(OrderState.Accepted))
                        return false;

                    order.ClosingDate = DateTime.Now;
                }

                order.State = state;
                db.SaveChanges();
                return true;
            }
        }
        #endregion

        #region IUserDataAccess
        public Order AddOrder(int userId, int tableId, int waiterId, IEnumerable<Tuple<int, int>> menuItems)
        {
            if (!CheckIsUserLoggedIn(waiterId))
                throw new SecurityException(String.Format("Waiter id={0} is not logged in.", waiterId));
            if (menuItems == null || !menuItems.Any())
                throw new ArgumentNullException("menuItems is null");

            Order order = null;

            using (var db = new DataAccessProvider())
            {
                Table table = db.Tables.Find(tableId);
                if (table == null)
                    throw new ArgumentException(String.Format("No such table (id={0}) exists.", tableId));

                UserContext waiter = db.Users.Find(waiterId);
                if (waiter == null)
                    throw new ArgumentException(String.Format("No such waiter (id={0}) exists.", waiterId));

                //Na pierwszym etapie ustawiamy stan zamówienia od razu na Accepted (kelner dodający zamówienie jednocześnie je akceptuje), aczkolwiek w drugim etapie będzie ustawiany stan OrderState.Placed
                order = new Order() { UserId = userId, Table = table, Waiter = waiter, State = OrderState.Accepted, PlacingDate = DateTime.Now, ClosingDate = DateTime.MaxValue };

                foreach (var tuple in menuItems)
                {
                    MenuItem menuItem = db.MenuItems.Find(tuple.Item1);
                    if (menuItem == null)
                        throw new ArgumentException(String.Format("No such menuItem (id={0}) exists", tuple.Item1));
                    if (tuple.Item2 <= 0)
                        throw new ArgumentException(String.Format("MenuItem id={0} has quantity={1}", tuple.Item1, tuple.Item2));

                    MenuItemQuantity menuItemQuantity = new MenuItemQuantity() { MenuItem = menuItem, Quantity = tuple.Item2 };
                    order.MenuItems.Add(menuItemQuantity);
                }

                order = db.Orders.Add(order);
                db.SaveChanges();
            }

            return order;
        }
        #endregion

        #region Private Methods

        private bool CheckIsUserLoggedIn(int userId)
        {
            return loggedInUsers.FirstOrDefault(c => c.Id.Equals(userId)) != null;
        }

        private bool CheckHasUserRole(int userId, UserRole role)
        {
            UserContext user = this.loggedInUsers.FirstOrDefault(c => c.Id.Equals(userId));
            if (user == null)
                return false;
            return (user.Role & role) != 0;
        }

        private bool RemoveFromLoggedInUsers(int userId)
        {
            UserContext userToRemove = this.loggedInUsers.FirstOrDefault(c => c.Id.Equals(userId));
            if (userToRemove == null)
                return false;

            this.loggedInUsers.Remove(userToRemove);
            return true;
        }

        #endregion

        #region IDataWipe

        bool IDataWipe.WipeMenuItemCategory(int categoryId)
        {
            using (var db = new DataAccessProvider())
            {
                MenuItemCategory menuItemCategoryToRemove = db.MenuItemCategories.Find(categoryId);
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
                MenuItem menuItemToRemove = db.MenuItems.Find(menuItemId);
                if (menuItemToRemove == null)
                    return false;

                db.MenuItems.Remove(menuItemToRemove);
                db.SaveChanges();
                return true;
            }        
        }

        bool IDataWipe.WipeUser(int userId)
        {
            using (var db = new DataAccessProvider())
            {
                UserContext userContextToRemove = db.Users.Find(userId);
                if (userContextToRemove == null)
                    return false;

                if (CheckIsUserLoggedIn(userContextToRemove.Id))
                    RemoveFromLoggedInUsers(userContextToRemove.Id);

                db.Users.Remove(userContextToRemove);
                db.SaveChanges();
                return true;
            }
        }

        bool IDataWipe.WipeTable(int tableId)
        {
            using (var db = new DataAccessProvider())
            {
                Table tableToRemove = db.Tables.Find(tableId);
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
                Order orderToRemove = db.Orders.Find(orderId);
                if (orderToRemove == null)
                    return false;

                var quantityList = orderToRemove.MenuItems.ToList();
                foreach (MenuItemQuantity quantity in quantityList)
                    db.MenuItemQuantities.Remove(quantity);

                db.Orders.Remove(orderToRemove);
                db.SaveChanges();
                return true;
            }
        }

        #endregion
    }
}
