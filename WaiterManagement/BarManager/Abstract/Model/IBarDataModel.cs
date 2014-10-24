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

        bool EditMenuItem(WaiterContext Waiter, string Login, string FirstName, string LastName, string Password);

        IList<Table> GetAllTables();

        bool DeleteTable(int id);

        Table AddTable(int Number, string TableDescription);

        bool EditTable(Table table, int number, string tableDescription);
    }
}
