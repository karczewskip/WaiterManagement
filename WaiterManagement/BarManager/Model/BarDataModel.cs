using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarManager.Abstract;
using DataAccess;
using ClassLib.DbDataStructures;
using System.Windows;
using System.Data.Common;
using System.Data.Odbc;

namespace BarManager.Model
{
    public class BarDataModel : IBarDataModel
    {
        private IManagerDataAccess ManagerDataAccess;

        public BarDataModel(IManagerDataAccess managerDataAccess)
        {
            ManagerDataAccess = managerDataAccess;
        }

        public IList<MenuItemCategory> GetAllCategories()
        {
            try
            {
                return ManagerDataAccess.GetMenuItemCategories().ToList();
            }
            catch
            {
                throw new Exception("Exception from DB");
            }
        }

        public MenuItemCategory AddCategoryItem(string categoryName, string categoryDescription)
        {
            MenuItemCategory AddingCategory;
            try
            {
                AddingCategory = ManagerDataAccess.AddMenuItemCategory(categoryName, categoryDescription);
            }
            catch
            {
                throw new Exception("Exception from DB");
            }
            return AddingCategory;
        }

        public IList<MenuItem> GetAllMenuItems()
        {
            try
            {
                return ManagerDataAccess.GetMenuItems().ToList();
            }
            catch
            {
                throw new Exception("Exception from DB");
            }
        }


        public MenuItem AddMenuItem(string menuItemName, MenuItemCategory category, double price, string menuItemDescription)
        {
            MenuItem AddingMenuItem;
            try
            {
                AddingMenuItem = ManagerDataAccess.AddMenuItem(menuItemName, menuItemDescription, category.Id, new Money() { Amount = (float)price, Currency = "PLN" });
            }
            catch
            {
                throw new Exception("Exception from DB");
            }

            if (AddingMenuItem != null)
            {
                AddingMenuItem.Category = category;
            }

            return AddingMenuItem;
        }


        public bool DeleteItem(int id)
        {
            bool result;
            try
            {
                result = ManagerDataAccess.RemoveMenuItem(id);
            }
            catch
            {
                throw new Exception("Exception from DB");
            }

            return result;
        }

        public bool EditMenuItem(MenuItem menuItemToEdit, string newName, double newPrice, MenuItemCategory newCategory, string newMenuItemDescription)
        {
            bool result;

            var oldName = menuItemToEdit.Name;
            var oldPrice = menuItemToEdit.Price;
            var oldCategory = menuItemToEdit.Category;
            var oldDescription = menuItemToEdit.Description;

            menuItemToEdit.Name = newName;
            menuItemToEdit.Price = new Money() { Amount = (float)newPrice, Currency = "PLN" };
            menuItemToEdit.Category = newCategory;
            menuItemToEdit.Description = newMenuItemDescription;

            try
            {
                result = ManagerDataAccess.EditMenuItem(menuItemToEdit);
            }
            catch
            {
                menuItemToEdit.Name = oldName;
                menuItemToEdit.Price = oldPrice;
                menuItemToEdit.Category = oldCategory;
                menuItemToEdit.Description = oldDescription;

                throw new Exception("Exception from DB");
            }

            if (!result)
            {
                menuItemToEdit.Name = oldName;
                menuItemToEdit.Price = oldPrice;
                menuItemToEdit.Category = oldCategory;
                menuItemToEdit.Description = oldDescription;
            }
            else
                menuItemToEdit.Category = newCategory;

            return result;
        }


        public IList<WaiterContext> GetAllWaiters()
        {
            try
            {
                return ManagerDataAccess.GetWaiters().ToList();
            }
            catch
            {
                throw new Exception("Exception from DB");
            }
        }

        public bool DeleteWaiter(int id)
        {
            bool result;
            try
            {
                result = ManagerDataAccess.RemoveWaiter(id);
            }
            catch
            {
                throw new Exception("Exception from DB");
            }

            return result;
        }


        public WaiterContext AddWaiter(string login, string firstName, string lastName, string password)
        {
            WaiterContext AddingWaiter;
            try
            {
                AddingWaiter = ManagerDataAccess.AddWaiter(firstName, lastName, login, password);
            }
            catch
            {
                throw new Exception("Exception from DB");
            }

            return AddingWaiter;
        }


        public bool EditWaiter(WaiterContext waiter, string login, string firstName, string lastName, string password)
        {
            bool result;

            var oldLogin = waiter.Login;
            var oldFirstName = waiter.FirstName;
            var oldSecondName = waiter.LastName;
            var oldPassword = waiter.Password;

            waiter.Login = login;
            waiter.FirstName = firstName;
            waiter.LastName = lastName;
            waiter.Password = password;

            try
            {
                result = ManagerDataAccess.EditWaiter(waiter);
            }
            catch
            {
                waiter.Login = oldLogin;
                waiter.FirstName = oldFirstName;
                waiter.LastName = oldSecondName;
                waiter.Password = oldPassword;

                throw new Exception("Exception from DB");
            }

            if (!result)
            {
                waiter.Login = oldLogin;
                waiter.FirstName = oldFirstName;
                waiter.LastName = oldSecondName;
                waiter.Password = oldPassword;
            }

            return result;
        }


        public IList<Table> GetAllTables()
        {
            try
            {
                return ManagerDataAccess.GetTables().ToList();
            }
            catch
            {
                throw new Exception("Exception from DB");
            }
        }

        public bool DeleteTable(int id)
        {
            bool result;
            try
            {
                result = ManagerDataAccess.RemoveTable(id);
            }
            catch
            {
                throw new Exception("Exception from DB");
            }

            return result;
        }


        public Table AddTable(int number, string tableDescription)
        {
            Table addingTable;
            try
            {
                addingTable = ManagerDataAccess.AddTable(number, tableDescription);
            }
            catch
            {
                throw new Exception("Exception from DB");
            }

            return addingTable;
        }


        public bool EditTable(Table table, int number, string tableDescription)
        {
            bool result;

            var oldNumber = table.Number;
            var oldDescription = table.Description;

            table.Number = number;
            table.Description = tableDescription;

            try
            {
                result = ManagerDataAccess.EditTable(table);
            }
            catch
            {
                table.Number = oldNumber;
                table.Description = oldDescription;

                throw new Exception("Exception from DB");
            }

            if (!result)
            {
                table.Number = oldNumber;
                table.Description = oldDescription;
            }

            return result;
        }
    }
}
