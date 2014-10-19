using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLib.DbDataStructures;

namespace DataAccess
{
    public class DataAccess : IManagerDataAccess
    {
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

        public MenuItem AddMenuItem(string name, string description, MenuItemCategory category, Money price)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("name is null");
            if (String.IsNullOrEmpty(description))
                throw new ArgumentNullException("description is null");
            if (category == null)
                throw new ArgumentNullException("category is null");

            MenuItem newMenuItem = null;

            using (var db = new DataAccessProvider())
            {
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
        
        public IEnumerable<Table> GetTables()
        {
            using(var db = new DataAccessProvider())
            {
                IList<Table> tableList = new List<Table>();
                foreach (Table table in db.Tables)
                    tableList.Add(table);
                return tableList;
            }
        }
    }
}
