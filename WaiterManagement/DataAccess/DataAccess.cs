using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLib.DbDataStructures;
using System.Security;

namespace DataAccess
{
    public class DataAccess : IManagerDataAccess, IWaiterDataAccess
    {
        #region Private Fields
        private HashSet<int> loggedInWaiterIds;
        #endregion

        #region Constructors
        public DataAccess()
        {
            loggedInWaiterIds = new HashSet<int>();
        }
        #endregion

        #region IBaseDataAccess
        public IEnumerable<MenuItemCategory> GetMenuItemCategories()
        {
            using (var db = new DataAccessProvider())
            {
                IList<MenuItemCategory> menuItemCategoryList = new List<MenuItemCategory>();
                foreach (MenuItemCategory menuItemCategory in db.MenuItemCategories)
                    menuItemCategoryList.Add(menuItemCategory);
                return menuItemCategoryList;
            }
        }

        public IEnumerable<MenuItem> GetMenuItems()
        {
            using (var db = new DataAccessProvider())
            {
                IList<MenuItem> menuItemList = new List<MenuItem>();
                foreach (MenuItem menuItem in db.MenuItems)
                    menuItemList.Add(menuItem);
                return menuItemList;
            }
        }

        public IEnumerable<Table> GetTables()
        {
            using (var db = new DataAccessProvider())
            {
                IList<Table> tableList = new List<Table>();
                foreach (Table table in db.Tables)
                    tableList.Add(table);
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
                db.MenuItemCategories.Add(newCategory);
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
                editedMenuItemCategory.CopyData(menuItemCategoryToEdit);
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
                db.MenuItemCategories.Remove(menuItemCategoryToRemove);
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

            using (var db = new DataAccessProvider())
            {
                MenuItemCategory category = db.MenuItemCategories.Find(categoryId);
                if (category == null)
                    return null;
                newMenuItem = new MenuItem() {Name = name, Description = description, Category = category, Price = price};
                db.MenuItems.Add(newMenuItem);
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
                editedMenuItem.CopyData(menuItemToEdit);
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
                db.MenuItems.Remove(menuItemToRemove);
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
                newWaiterContext = new WaiterContext() { FirstName = firstName, LastName = lastName, Login = login, Password = password };
                db.Waiters.Add(newWaiterContext);
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
                editedWaiterContext.CopyData(waiterToEdit);
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
                db.Waiters.Remove(waiterContextToRemove);
                db.SaveChanges();
                return true;
            }
        }

        public IEnumerable<WaiterContext> GetWaiters()
        {
            using(var db = new DataAccessProvider())
            {
                IList<WaiterContext> waiterList = new List<WaiterContext>();
                foreach (WaiterContext waiterContext in db.Waiters)
                    waiterList.Add(waiterContext);
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
                db.Tables.Add(newTable);
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

                editedTable.CopyData(tableToEdit);
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
                db.Tables.Remove(tableToRemove);
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
                loggedInWaiterIds.Add(waiterContext.Id);

            return waiterContext;
        }

        public bool LogOut(int waiterId)
        {
            if (!loggedInWaiterIds.Contains(waiterId))
                return false;
            loggedInWaiterIds.Remove(waiterId);
            return true;
        }

        public Order AddOrder(int userId, int tableId, int waiterId, IEnumerable<Tuple<int, int>> menuItems)
        {
            if (!loggedInWaiterIds.Contains(waiterId))
                throw new SecurityException(String.Format("Waiter id={0} is not logged in.", waiterId));
            if (menuItems == null || !menuItems.Any())
                throw new ArgumentNullException("menuItems is null");

            Order order = null;

            using(var db = new DataAccessProvider())
            {
                Table table = db.Tables.Find(tableId);
                if (table == null)
                    throw new ArgumentException(String.Format("No such table (id={0}) exists.", tableId));
                            
                foreach(var tuple in menuItems)
                {
                    MenuItem menuItem = db.MenuItems.Find(tuple.Item1);
                    if (menuItem == null)
                        throw new ArgumentException(String.Format("No such menuItem (id={0}) exists", tuple.Item1));
                    if (tuple.Item2 <= 0)
                        throw new ArgumentException(String.Format("MenuItem id={0} has quantity={1}", tuple.Item1, tuple.Item2));
                }

                order = new Order() { UserId = userId, TableId = tableId, WaiterId = waiterId };
                foreach (var tuple in menuItems)
                    order.MenuItems.Add(tuple);
                db.Orders.Add(order);
                db.SaveChanges();
            }

            return order;
        }

        public IEnumerable<Order> GetPastOrders(int waiterId)
        {
            if(!loggedInWaiterIds.Contains(waiterId))
                throw new SecurityException(String.Format("Waiter id={0} is not logged in.", waiterId));

            IEnumerable<Order> orders = null;

            using(var db = new DataAccessProvider())
            {
                orders = db.Orders.Where(o => o.WaiterId == waiterId).ToList();
            }

            return orders;
        }
        #endregion
    }
}
