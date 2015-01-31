using System;
using System.Collections.Generic;
using System.Linq;
using ClassLib;
using BarManager.ManagerDataAccessWCFService;
using BarManager.Abstract;
using BarManager.Abstract.Model;

namespace BarManager.Model
{
    public class MenuDataModel : IMenuDataModel
    {
        private readonly IManagerDataAccess _managerDataAccess;

        public MenuDataModel(IManagerDataAccess managerDataAccess)
        {
            _managerDataAccess = managerDataAccess;
        }

        public IList<MenuItemCategory> GetAllCategories()
        {
            try
            {
                return _managerDataAccess.GetMenuItemCategories().ToList();
            }
            catch
            {
                throw new Exception("Exception from DB");
            }
        }

        public MenuItemCategory AddCategoryItem(string categoryName, string categoryDescription)
        {
            MenuItemCategory addingCategory = null;
            try
            {
                addingCategory = _managerDataAccess.AddMenuItemCategory( categoryName, categoryDescription);
            }
            catch
            {
                throw new Exception("Exception from DB");
            }
            return addingCategory;
        }

        public IList<MenuItem> GetAllMenuItems()
        {
            try
            {
                return _managerDataAccess.GetMenuItems().ToList();
            }
            catch
            {
                throw new Exception("Exception from DB");
            }
        }

        public MenuItem AddMenuItem(string menuItemName, MenuItemCategory category, double price, string menuItemDescription)
        {
            MenuItem addingMenuItem;
            try
            {
                addingMenuItem = _managerDataAccess.AddMenuItem( menuItemName, menuItemDescription, category.Id, new Money() { Amount = (float)price, Currency = ApplicationResources.DefaultCurrency });
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
            bool result;
            try
            {
                result = _managerDataAccess.RemoveMenuItem( id);
            }
            catch
            {
                throw new Exception("Exception from DB");
            }

            return result;
        }

        public bool EditMenuItem(MenuItem menuItem, string newName, double newPrice, MenuItemCategory newCategory, string newMenuItemDescription)
        {
            bool result;

            var oldName = menuItem.Name;
            var oldPrice = menuItem.Price;
            var oldCategory = menuItem.Category;
            var oldDescription = menuItem.Description;

            menuItem.Name = newName;
            menuItem.Price = new Money() { Amount = (float)newPrice, Currency = ApplicationResources.DefaultCurrency };
            menuItem.Category = newCategory;
            menuItem.Description = newMenuItemDescription;

            try
            {
                result = _managerDataAccess.EditMenuItem( menuItem);
            }
            catch
            {
                menuItem.Name = oldName;
                menuItem.Price = oldPrice;
                menuItem.Category = oldCategory;
                menuItem.Description = oldDescription;

                throw new Exception("Exception from DB");
            }

            if (!result)
            {
                menuItem.Name = oldName;
                menuItem.Price = oldPrice;
                menuItem.Category = oldCategory;
                menuItem.Description = oldDescription;
            }
            else
                menuItem.Category = newCategory;

            return result;
        }
    }
}