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
    class BarDataModel : IBarDataModel
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
                MessageBox.Show("Failed");
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
                MessageBox.Show("Failed");
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
            try
            {
                ManagerDataAccess.RemoveMenuItem(id);
            }
            catch
            {
                return false;
            }

            return true;
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
    }
}
