using System.Collections.Generic;
using BarManager.ManagerDataAccessWCFService;

namespace BarManager.Abstract.Model
{
    public interface IMenuDataModel
    {
        MenuItemCategory AddCategoryItem(string CategoryName, string CategoryDescription);

        MenuItem AddMenuItem(string menuItemName, MenuItemCategory selectedCategory, double price,
            string menuItemDescription);

        bool EditMenuItem(MenuItem menuItem, string MenuItemName, double price, MenuItemCategory SelectedCategory,
            string MenuItemDescription);

        IList<MenuItemCategory> GetAllCategories();

        IList<MenuItem> GetAllMenuItems();

        bool DeleteItem(int id);
    }
}