using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarManager.Abstract;
using DataAccess;
using ClassLib.DbDataStructures;
using System.Windows;

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
            return ManagerDataAccess.GetMenuItemCategories().ToList();
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
                return null;
            }
            return AddingCategory;
        }

        public IList<MenuItem> GetAllMenuItems()
        {
            return ManagerDataAccess.GetMenuItems().ToList();
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
                return null;
            }

            if(AddingMenuItem != null)
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
                return false;
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

                return false;
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
            return ManagerDataAccess.GetWaiters().ToList();
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
                return false;
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
                return null;
            }

            return AddingWaiter;
        }


        public bool EditWaiter(WaiterContext Waiter, string Login, string FirstName, string LastName, string Password)
        {
            bool result;

            var oldLogin = Waiter.Login;
            var oldFirstName = Waiter.FirstName;
            var oldSecondName = Waiter.LastName;
            var oldPassword = Waiter.Password;

            Waiter.Login = Login;
            Waiter.FirstName = FirstName;
            Waiter.LastName = LastName;
            Waiter.Password = Password;

            try
            {
                result = ManagerDataAccess.EditWaiter(Waiter);
            }
            catch
            {
                Waiter.Login = oldLogin;
                Waiter.FirstName = oldFirstName;
                Waiter.LastName = oldSecondName;
                Waiter.Password = oldPassword;

                return false;
            }

            if (!result)
            {
                Waiter.Login = oldLogin;
                Waiter.FirstName = oldFirstName;
                Waiter.LastName = oldSecondName;
                Waiter.Password = oldPassword;
            }

            return result;
        }


        public IList<Table> GetAllTables()
        {
            return ManagerDataAccess.GetTables().ToList();
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
                return false;
            }

            return result;
        }


        public Table AddTable(int number, string tableDescription)
        {
            Table AddingTable;
            try
            {
                AddingTable = ManagerDataAccess.AddTable(number, tableDescription);
            }
            catch
            {
                return null;
            }

            return AddingTable;
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

                return false;
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
