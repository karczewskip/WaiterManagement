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

        IList<WaiterContext> GetAllWaiters();

        bool DeleteWaiter(int id);

        WaiterContext AddWaiter(string login, string firstName, string lastName, string password);

        bool EditWaiter(WaiterContext waiter, string login, string firstName, string lastName, string password);

        IList<Table> GetAllTables();

        bool DeleteTable(int id);

        Table AddTable(int number, string tableDescription);

        bool EditTable(Table table, int number, string tableDescription);
    }
}
