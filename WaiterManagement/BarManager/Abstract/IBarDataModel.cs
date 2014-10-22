using ClassLib.DbDataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarManager.Abstract
{
    public interface IBarDataModel
    {
        IList<MenuItemCategory> GetAllCategories();
        MenuItemCategory AddCategoryItem(string categoryName, string categoryDescription);

        IList<MenuItem> GetAllMenuItems();

        MenuItem AddMenuItem(string menuItemName, MenuItemCategory category, double price, string menuItemDescription);

        bool DeleteItem(int id);

        bool EditMenuItem(MenuItem menuItemToEdit, string newName, double newPrice, MenuItemCategory newCategory, string newMenuItemDescription);
    }
}
