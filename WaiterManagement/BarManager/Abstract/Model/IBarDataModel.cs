using BarManager.ManagerDataAccessWCFService;
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

        IList<UserContext> GetAllWaiters();

        bool DeleteWaiter(int id);

        UserContext AddWaiter(string login, string firstName, string lastName, string password);

        bool EditWaiter(UserContext waiter, string login, string firstName, string lastName, string password);

        IList<Table> GetAllTables();

        bool DeleteTable(int id);

        Table AddTable(int number, string tableDescription);

        bool EditTable(Table table, int number, string tableDescription);

        bool IsLogged();

        void LogIn();
    }
}
