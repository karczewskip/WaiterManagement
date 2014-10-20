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


        public MenuItem AddMenuItem(string menuItemName, int categoryId, double price, string menuItemDescription)
        {
            MenuItem AddingMenuItem;
            try
            {
                //AddingMenuItem = ManagerDataAccess.AddMenuItem()
            }
            catch
            {

            }

            return null;
        }
    }
}
