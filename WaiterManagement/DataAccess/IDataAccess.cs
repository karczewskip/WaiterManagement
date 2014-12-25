using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLib;
using ClassLib.DbDataStructures;

namespace DataAccess
{
    /// <summary>
    /// Zbiór funkcji stanowiący podstawowy dostęp do danych
    /// </summary>
    public interface IBaseDataAccess
    {
        IEnumerable<MenuItemCategory> GetMenuItemCategories();
        IEnumerable<MenuItem> GetMenuItems();
        IEnumerable<Table> GetTables();
    }

    /// <summary>
    /// Zbiór funkcji składający się na funkcjonalność manadżera
    /// </summary>
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

    /// <summary>
    /// Zbiór funkcji składający się na funkcjonalność kelnera.
    /// </summary>
    public interface IWaiterDataAccess : IBaseDataAccess
    {
        WaiterContext LogIn(string login, string password);
        bool LogOut(int waiterId);
        Order AddOrder(int userId, int tableId, int waiterId, IEnumerable<Tuple<int, int>> menuItems);
        IEnumerable<Order> GetPastOrders(int waiterId);
        IEnumerable<Order> GetPastOrders(int waiterId, int firstIndex, int lastIndex);
        IEnumerable<Order> GetActiveOrders(int waiterId);
        bool SetOrderState(int waiterId, int orderId, OrderState state);        
    }

    /// <summary>
    /// Interfejs używany przez metody testujący do sprzątania po sobie
    /// </summary>
    public interface IDataWipe
    {
        bool WipeMenuItemCategory(int categoryId);
        bool WipeMenuItem(int menuItemId);
        bool WipeWaiter(int waiterId);
        bool WipeTable(int tableId);
        bool WipeOrder(int orderId);
    }
}
