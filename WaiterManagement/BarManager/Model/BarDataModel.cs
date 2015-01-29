using BarManager.Abstract;
using BarManager.ManagerDataAccessWCFService;
using System;
using System.Collections.Generic;
using System.Linq;
using ClassLib;

namespace BarManager.Model
{
    public class BarDataModel : IBarDataModel, IDisposable
    {
        private IManagerDataAccess ManagerDataAccess;
        private bool access = false;
        private UserContext managerUserContext;

        public BarDataModel(IManagerDataAccess managerDataAccess)
        {
            ManagerDataAccess = managerDataAccess;
        }

        public IList<MenuItemCategory> GetAllCategories()
        {
            if (managerUserContext == null)
                return null;
            try
            {
                return ManagerDataAccess.GetMenuItemCategories(managerUserContext.Id).ToList();
            }
            catch
            {
                throw new Exception("Exception from DB");
            }
        }

        public MenuItemCategory AddCategoryItem(string categoryName, string categoryDescription)
        {
            if (managerUserContext == null)
                return null;

            MenuItemCategory addingCategory = null;
            try
            {
                addingCategory = ManagerDataAccess.AddMenuItemCategory(managerUserContext.Id, categoryName, categoryDescription);
            }
            catch
            {
                throw new Exception("Exception from DB");
            }
            return addingCategory;
        }

        public IList<MenuItem> GetAllMenuItems()
        {
            if (managerUserContext == null)
                return null;
            try
            {
                return ManagerDataAccess.GetMenuItems(managerUserContext.Id).ToList();
            }
            catch
            {
                throw new Exception("Exception from DB");
            }
        }

        public MenuItem AddMenuItem(string menuItemName, MenuItemCategory category, double price, string menuItemDescription)
        {
            if (managerUserContext == null)
                return null;

            MenuItem addingMenuItem;
            try
            {
                addingMenuItem = ManagerDataAccess.AddMenuItem(managerUserContext.Id, menuItemName, menuItemDescription, category.Id, new Money() { Amount = (float)price, Currency = ApplicationResources.DefaultCurrency });
            }
            catch
            {
                throw new Exception("Exception from DB");
            }

            //if (addingMenuItem != null)
            //{
            //    addingMenuItem.Category = category;
            //}

            return addingMenuItem;
        }

        public bool DeleteItem(int id)
        {
            if (managerUserContext == null)
                return false;

            bool result;
            try
            {
                result =  ManagerDataAccess.RemoveMenuItem(managerUserContext.Id, id);
            }
            catch
            {
                throw new Exception("Exception from DB");
            }

            return result;
        }

        public bool EditMenuItem(MenuItem menuItemToEdit, string newName, double newPrice, MenuItemCategory newCategory, string newMenuItemDescription)
        {
            if (managerUserContext == null)
                return false;

            bool result;

            var oldName = menuItemToEdit.Name;
            var oldPrice = menuItemToEdit.Price;
            var oldCategory = menuItemToEdit.Category;
            var oldDescription = menuItemToEdit.Description;

            menuItemToEdit.Name = newName;
            menuItemToEdit.Price = new Money() { Amount = (float)newPrice, Currency = ApplicationResources.DefaultCurrency };
            menuItemToEdit.Category = newCategory;
            menuItemToEdit.Description = newMenuItemDescription;

            try
            {
                result =  ManagerDataAccess.EditMenuItem(managerUserContext.Id, menuItemToEdit);
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

        public IList<UserContext> GetAllWaiters()
        {
            if (managerUserContext == null)
                return null;

            try
            {
                return ManagerDataAccess.GetWaiters(managerUserContext.Id).ToList();
            }
            catch
            {
                throw new Exception("Exception from DB");
            }
        }

        public bool DeleteWaiter(int id)
        {
            if (managerUserContext == null)
                return false;

            bool result;
            try
            {
                result = ManagerDataAccess.RemoveWaiter(managerUserContext.Id, id);
            }
            catch
            {
                throw new Exception("Exception from DB");
            }

            return result;
        }

        public UserContext AddWaiter(string login, string firstName, string lastName, string password)
        {
            if (managerUserContext == null)
                return null;

            UserContext addingWaiter;
            try
            {
                addingWaiter = ManagerDataAccess.AddWaiter(managerUserContext.Id, firstName, lastName, login, ClassLib.DataStructures.HashClass.CreateFirstHash(password, login));
            }
            catch(Exception e)
            {
                throw new Exception("Exception from DB");
            }

            return addingWaiter;
        }

        public bool EditWaiter(UserContext waiter, string login, string firstName, string lastName, string password)
        {
            if (managerUserContext == null)
                return false;

            bool result;

            var oldLogin = waiter.Login;
            var oldFirstName = waiter.FirstName;
            var oldSecondName = waiter.LastName;
            //var oldPassword = waiter.Password;

            waiter.Login = login;
            waiter.FirstName = firstName;
            waiter.LastName = lastName;
            //waiter.Password = password;

            try
            {
                result = ManagerDataAccess.EditWaiter(managerUserContext.Id, waiter);
            }
            catch
            {
                waiter.Login = oldLogin;
                waiter.FirstName = oldFirstName;
                waiter.LastName = oldSecondName;
                //waiter.Password = oldPassword;

                throw new Exception("Exception from DB");
            }

            if (!result)
            {
                waiter.Login = oldLogin;
                waiter.FirstName = oldFirstName;
                waiter.LastName = oldSecondName;
                //waiter.Password = oldPassword;
            }

            return result;
        }

        public IList<Table> GetAllTables()
        {
            if (managerUserContext == null)
                return null;

            try
            {
                return ManagerDataAccess.GetTables(managerUserContext.Id).ToList();
            }
            catch
            {
                throw new Exception("Exception from DB");
            }
        }

        public bool DeleteTable(int id)
        {
            if (managerUserContext == null)
                return false;

            bool result;
            try
            {
                result =  ManagerDataAccess.RemoveTable(managerUserContext.Id, id);
            }
            catch
            {
                throw new Exception("Exception from DB");
            }

            return result;
        }

        public Table AddTable(int number, string tableDescription)
        {
            if (managerUserContext == null)
                return null;

            Table addingTable;
            try
            {
                addingTable = ManagerDataAccess.AddTable(managerUserContext.Id, number, tableDescription);
            }
            catch
            {
                throw new Exception("Exception from DB");
            }

            return addingTable;
        }

        public bool EditTable(Table table, int number, string tableDescription)
        {
            if (managerUserContext == null)
                return false;

            bool result;

            var oldNumber = table.Number;
            var oldDescription = table.Description;

            table.Number = number;
            table.Description = tableDescription;

            try
            {
                result = ManagerDataAccess.EditTable(managerUserContext.Id, table);
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

        public bool IsLogged()
        {
            return managerUserContext != null;
        }

        public void LogIn(string login, string password)
        {
            managerUserContext = ManagerDataAccess.LogIn(login, ClassLib.DataStructures.HashClass.CreateFirstHash(password, login));
        }

        public void Register(string firstName, string lastName ,string login, string password)
        {
            ManagerDataAccess.AddManager(firstName, lastName, login, ClassLib.DataStructures.HashClass.CreateFirstHash(password, login));
            LogIn(login, password);
        }

        public void Dispose()
        {
            ManagerDataAccess.Close();
        }
    }
}
