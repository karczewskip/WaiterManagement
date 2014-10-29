using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLib;
using ClassLib.DbDataStructures;

namespace DataAccess
{
    public interface IBaseDataAccess
    {
        IEnumerable<MenuItemCategory> GetMenuItemCategories();
        IEnumerable<MenuItem> GetMenuItems();
        IEnumerable<Table> GetTables();
    }
    public interface IManagerDataAccess : IBaseDataAccess
    {
        MenuItemCategory AddMenuItemCategory(string name, string description);
        bool EditMenuItemCategory(MenuItemCategory menuItemCategoryToEdit);
        bool RemoveMenuItemCategory(int categoryId);
        MenuItem AddMenuItem(string name, string description, int categoryId, Money price);
        bool EditMenuItem(MenuItem menuItemToEdit);
        bool RemoveMenuItem(int menuItemId);
        WaiterContext AddWaiter(string firstName, string lastName, string login, string password);
        bool EditWaiter(WaiterContext waiterToEdit);
        bool RemoveWaiter(int waiterId);
        IEnumerable<WaiterContext> GetWaiters();
        Table AddTable(int tableNumber, string description);
        bool EditTable(Table tableToEdit);
        bool RemoveTable(int tableId);
        IEnumerable<Order> GetOrders();
        bool RemoveOrder(int orderId);
    }

    public interface IWaiterDataAccess : IBaseDataAccess
    {
        WaiterContext LogIn(string login, string password);
        bool LogOut(int waiterId);
        Order AddOrder(int userId, int tableId, int waiterId, IEnumerable<Tuple<int, int>> menuItems);
        IEnumerable<Order> GetPastOrders(int waiterId);
    }
}
