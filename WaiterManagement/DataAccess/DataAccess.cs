using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLib.DbDataStructures;
using System.Security;
using System.Data.Entity;
using DataAccess.Migrations;

namespace DataAccess
{
    /// <summary>
    /// Klasa agregująca metody dostępu do bazy danych
    /// </summary>
    public class DataAccessClass : IManagerDataAccess, IWaiterDataAccess
    {
        #region Private Fields
        private HashSet<int> loggedInWaiterIds;
        #endregion

        #region Constructors
        public DataAccessClass()
        {
            loggedInWaiterIds = new HashSet<int>();
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataAccessProvider, Configuration>()); 
        }
        #endregion

        #region IBaseDataAccess
        public IEnumerable<MenuItemCategory> GetMenuItemCategories()
        {
            using (var db = new DataAccessProvider())
            {
                var menuItemCategoryList = db.MenuItemCategories.ToList();
                return menuItemCategoryList;
            }
        }

        public IEnumerable<MenuItem> GetMenuItems()
        {
            using (var db = new DataAccessProvider())
            {
                var menuItemList = db.MenuItems.Include("Category").ToList();
                return menuItemList;
            }
        }

        public IEnumerable<Table> GetTables()
        {
            using (var db = new DataAccessProvider())
            {
                var tableList = db.Tables.ToList();
                return tableList;
            }
        }
        #endregion

        #region IManagerDataAccess
        public MenuItemCategory AddMenuItemCategory(string name, string description)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("Name is null");
            if (String.IsNullOrEmpty(description))
                throw new ArgumentNullException("Description is null");

            MenuItemCategory newCategory = null;

            using( var db = new DataAccessProvider())
            {
                newCategory = new MenuItemCategory() {Name = name, Description = description};
                newCategory = db.MenuItemCategories.Add(newCategory);
                db.SaveChanges();
            }

            return newCategory;
        }

        public bool EditMenuItemCategory(MenuItemCategory menuItemCategoryToEdit)
        {
            if (menuItemCategoryToEdit == null)
                throw new ArgumentNullException("menuItemCategoryToEdit is null");
            using(var db = new DataAccessProvider())
            {
                MenuItemCategory editedMenuItemCategory = db.MenuItemCategories.Find(menuItemCategoryToEdit.Id);
                if (editedMenuItemCategory == null)
                    return false;
                db.Entry(editedMenuItemCategory).State = System.Data.Entity.EntityState.Detached;
                db.MenuItemCategories.Attach(menuItemCategoryToEdit);
                db.Entry(menuItemCategoryToEdit).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return true;
            }
        }

        public bool RemoveMenuItemCategory(int categoryId)
        {
            using(var db = new DataAccessProvider())
            {
                MenuItemCategory menuItemCategoryToRemove = db.MenuItemCategories.Find(categoryId);
                if (menuItemCategoryToRemove == null)
                    return false;
                
                //db.MenuItemCategories.Remove(menuItemCategoryToRemove);
                menuItemCategoryToRemove.IsDeleted = true;
                db.SaveChanges();
                return true;
            }
        }

        public MenuItem AddMenuItem(string name, string description, int categoryId, Money price)
        {
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
                newMenuItem = new MenuItem() {Name = name, Description = description, Category = category, Price = price};
                newMenuItem = db.MenuItems.Add(newMenuItem);
                db.SaveChanges();
            }

            return newMenuItem;
        }

        public bool EditMenuItem(MenuItem menuItemToEdit)
        {
            if (menuItemToEdit == null)
                throw new ArgumentNullException("menuItemToEdit is null");
            
            using(var db = new DataAccessProvider())
            {
                MenuItem editedMenuItem = db.MenuItems.Find(menuItemToEdit.Id);
                if (editedMenuItem == null)
                    return false;
                db.Entry(editedMenuItem).State = System.Data.Entity.EntityState.Detached;
                db.MenuItems.Attach(menuItemToEdit);
                db.Entry(menuItemToEdit).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return true;
            }

        }

        public bool RemoveMenuItem(int menuItemId)
        {            
            using(var db = new DataAccessProvider())
            {
                MenuItem menuItemToRemove = db.MenuItems.Find(menuItemId);
                if (menuItemToRemove == null)
                    return false;
                
                //db.MenuItems.Remove(menuItemToRemove);
                menuItemToRemove.IsDeleted = true;
                db.SaveChanges();
                return true;
            }            
        }        

        public WaiterContext AddWaiter(string firstName, string lastName, string login, string password)
        {
            if (String.IsNullOrEmpty(firstName))
                throw new ArgumentNullException("firstName is null");
            if (String.IsNullOrEmpty(lastName))
                throw new ArgumentNullException("lastName is null");
            if (String.IsNullOrEmpty(login))
                throw new ArgumentNullException("login is null");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password is null");

            WaiterContext newWaiterContext = null;

            using (var db = new DataAccessProvider())
            {
                var waiterSameLogin = db.Waiters.Where(w => w.Login.Equals(login));

                if (waiterSameLogin != null && waiterSameLogin.Any())
                    throw new ArgumentException(String.Format("login = {0} already exists in database!", login));

                newWaiterContext = new WaiterContext() { FirstName = firstName, LastName = lastName, Login = login, Password = password };
                newWaiterContext = db.Waiters.Add(newWaiterContext);
                db.SaveChanges();
            }

            return newWaiterContext;
        }

        public bool EditWaiter(WaiterContext waiterToEdit)
        {
            if (waiterToEdit == null)
                throw new ArgumentNullException("waiterToEdit is null");

            using(var db = new DataAccessProvider())
            {
                WaiterContext editedWaiterContext = db.Waiters.Find(waiterToEdit.Id);
                if (editedWaiterContext == null)
                    return false;
                db.Entry(editedWaiterContext).State = System.Data.Entity.EntityState.Detached;
                db.Waiters.Attach(waiterToEdit);
                db.Entry(waiterToEdit).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return true;
            }
        }

        public bool RemoveWaiter(int waiterId)
        {
            using(var db = new DataAccessProvider())
            {
                WaiterContext waiterContextToRemove = db.Waiters.Find(waiterId);
                if (waiterContextToRemove == null)
                    return false;

                if (CheckIsWaiterLoggedIn(waiterContextToRemove.Id))
                    loggedInWaiterIds.Remove(waiterContextToRemove.Id);

                //db.Waiters.Remove(waiterContextToRemove);
                waiterContextToRemove.IsDeleted = true;
                db.SaveChanges();
                return true;
            }
        }

        public IEnumerable<WaiterContext> GetWaiters()
        {
            using(var db = new DataAccessProvider())
            {
                var waiterList = db.Waiters.ToList();
                return waiterList;
            }
        }

        public Table AddTable(int tableNumber, string description)
        {
            if (String.IsNullOrEmpty(description))
                throw new ArgumentNullException("description is null");

            Table newTable = null;

            using (var db = new DataAccessProvider())
            {
                newTable = new Table() { Number = tableNumber, Description = description };
                newTable = db.Tables.Add(newTable);
                db.SaveChanges();
            }

            return newTable;
        }

        public bool EditTable(Table tableToEdit)
        {
            if (tableToEdit == null)
                throw new ArgumentNullException("tableToEdit is null");

            using(var db = new DataAccessProvider())
            {
                Table editedTable = db.Tables.Find(tableToEdit.Id);
                if (editedTable == null)
                    return false;
                db.Entry(editedTable).State = System.Data.Entity.EntityState.Detached;
                db.Tables.Attach(tableToEdit);
                db.Entry(tableToEdit).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return true;
            }
        }

        public bool RemoveTable(int tableId)
        {
            using(var db = new DataAccessProvider())
            {
                Table tableToRemove = db.Tables.Find(tableId);
                if(tableToRemove == null)
                    return false;

                //db.Tables.Remove(tableToRemove);
                tableToRemove.IsDeleted = true;
                db.SaveChanges();
                return true;
            }
        }        

        public IEnumerable<Order> GetOrders()
        {
            using(var db = new DataAccessProvider())
            {
                var orderList = db.Orders.Include("Waiter").Include("Table").Include("MenuItems").ToList();
                return orderList;
            }
        }

        public bool RemoveOrder(int orderId)
        {
            using(var db = new DataAccessProvider())
            {
                Order orderToRemove = db.Orders.Find(orderId);
                if (orderToRemove == null)
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
        public WaiterContext LogIn(string login, string password)
        {
            if (String.IsNullOrEmpty(login))
                throw new ArgumentNullException("login is null");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password is null");

            WaiterContext waiterContext = null; 

            using(var db = new DataAccessProvider())
            {
                waiterContext = db.Waiters.Where(w => w.Login.Equals(login) && w.Password.Equals(password)).FirstOrDefault();
            }

            if (waiterContext != null)
            {
                if (CheckIsWaiterLoggedIn(waiterContext.Id))
                    throw new SecurityException(String.Format("Waiter id={0} is already logged in", waiterContext.Id));
                loggedInWaiterIds.Add(waiterContext.Id);
            }

            return waiterContext;
        }

        public bool LogOut(int waiterId)
        {
            if (!CheckIsWaiterLoggedIn(waiterId))
                return false;
            loggedInWaiterIds.Remove(waiterId);
            return true;
        }

        public Order AddOrder(int userId, int tableId, int waiterId, IEnumerable<Tuple<int, int>> menuItems)
        {
            if (!CheckIsWaiterLoggedIn(waiterId))
                throw new SecurityException(String.Format("Waiter id={0} is not logged in.", waiterId));
            if (menuItems == null || !menuItems.Any())
                throw new ArgumentNullException("menuItems is null");

            Order order = null;

            using(var db = new DataAccessProvider())
            {
                Table table = db.Tables.Find(tableId);
                if (table == null)
                    throw new ArgumentException(String.Format("No such table (id={0}) exists.", tableId));

                WaiterContext waiter = db.Waiters.Find(waiterId);
                if (waiter == null)
                    throw new ArgumentException(String.Format("No such waiter (id={0}) exists.", waiterId));

                //Na pierwszym etapie ustawiamy stan zamówienia od razu na Accepted (kelner dodający zamówienie jednocześnie je akceptuje), aczkolwiek w drugim etapie będzie ustawiany stan OrderState.Placed
                order = new Order() { UserId = userId, Table = table, Waiter = waiter, State = OrderState.Accepted, PlacingDate = DateTime.Now, ClosingDate = DateTime.MaxValue};
                            
                foreach(var tuple in menuItems)
                {
                    MenuItem menuItem = db.MenuItems.Find(tuple.Item1);
                    if (menuItem == null)
                        throw new ArgumentException(String.Format("No such menuItem (id={0}) exists", tuple.Item1));
                    if (tuple.Item2 <= 0)
                        throw new ArgumentException(String.Format("MenuItem id={0} has quantity={1}", tuple.Item1, tuple.Item2));

                    MenuItemQuantity menuItemQuantity = new MenuItemQuantity() { MenuItem = menuItem, Quantity = tuple.Item2};
                    order.MenuItems.Add(menuItemQuantity);
                }
                   
                order = db.Orders.Add(order);
                db.SaveChanges();
            }

            return order;
        }

        public IEnumerable<Order> GetPastOrders(int waiterId)
        {
            if(!CheckIsWaiterLoggedIn(waiterId))
                throw new SecurityException(String.Format("Waiter id={0} is not logged in.", waiterId));

            using(var db = new DataAccessProvider())
            {
                var orders = db.Orders.Include("MenuItems").Include("MenuItems.MenuItem").Include("MenuItems.MenuItem.Category").Include("Waiter").Include("Table").Where(o => o.Waiter.Id == waiterId && (o.State == OrderState.NotRealized || o.State == OrderState.Realized)).ToList();
                return orders;
            }

        }

        public IEnumerable<Order> GetPastOrders(int waiterId, int firstIndex, int lastIndex)
        {
            if(!CheckIsWaiterLoggedIn(waiterId))
                throw new SecurityException(String.Format("Waiter id={0} is not logged in.", waiterId));
            if(firstIndex < 0)
                throw new ArgumentException(String.Format("firstIndex ({0]) is smaller than zero", firstIndex));
            if(lastIndex < 0)
                throw new ArgumentException(String.Format("lastIndex ({0]) is smaller than zero", lastIndex));
            if (firstIndex > lastIndex)
                throw new ArgumentException(String.Format("firstIndex ({0}) is larger than lastIndex ({1})", firstIndex, lastIndex));

            using(var db = new DataAccessProvider())
            {
                var sortedList = db.Orders.Include("MenuItems").Include("MenuItems.MenuItem").Include("MenuItems.MenuItem.Category").Include("Waiter").Include("Table").Where(o => o.Waiter.Id == waiterId && (o.State == OrderState.Realized || o.State == OrderState.NotRealized)).OrderByDescending(o => o.ClosingDate).ToList();
                
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
            if(!CheckIsWaiterLoggedIn(waiterId))
                throw new SecurityException(String.Format("Waiter id={0} is not logged in.", waiterId));

            using(var db = new DataAccessProvider())
            {
                var activeOrders = db.Orders.Include("MenuItems").Include("MenuItems.MenuItem").Include("MenuItems.MenuItem.Category").Include("Waiter").Include("Table").Where( o => o.Waiter.Id == waiterId && (o.State == OrderState.Accepted || o.State == OrderState.Placed )).ToList();
                
                return activeOrders;
            }
        }

        public bool SetOrderState(int waiterId, int orderId, OrderState state)
        {
            if (!CheckIsWaiterLoggedIn(waiterId))
                throw new SecurityException(String.Format("Waiter id={0} is not logged in", waiterId));
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

        #region Private Methods

        private bool CheckIsWaiterLoggedIn(int waiterId)
        {
            return loggedInWaiterIds.Contains(waiterId);
        }

        #endregion
    }
}
