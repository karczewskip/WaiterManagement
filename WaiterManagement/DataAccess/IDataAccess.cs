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
        IEnumerable<MenuItemCategory> GetMenuItemCategories(int userId);
        IEnumerable<MenuItem> GetMenuItems(int userId);
        IEnumerable<Table> GetTables(int userId);
        UserContext LogIn(string login, string password);
        bool LogOut(int userId);
    }

    /// <summary>
    /// Zbiór funkcji składający się na funkcjonalność manadżera
    /// </summary>
    public interface IManagerDataAccess : IBaseDataAccess
    {
        UserContext AddManager(string firstName, string lastName, string login, string password);
        MenuItemCategory AddMenuItemCategory(int managerId, string name, string description);
        bool EditMenuItemCategory(int managerId, MenuItemCategory menuItemCategoryToEdit);
        bool RemoveMenuItemCategory(int managerId, int categoryId);
        MenuItem AddMenuItem(int managerId, string name, string description, int categoryId, Money price);
        bool EditMenuItem(int managerId, MenuItem menuItemToEdit);
        bool RemoveMenuItem(int managerId, int menuItemId);
        UserContext AddWaiter(int managerId, string firstName, string lastName, string login, string password);
        bool EditWaiter(int managerId, UserContext waiterToEdit);
        bool RemoveWaiter(int managerId, int waiterId);
        IEnumerable<UserContext> GetWaiters(int managerId);
        Table AddTable(int managerId, int tableNumber, string description);
        bool EditTable(int managerId, Table tableToEdit);
        bool RemoveTable(int managerId, int tableId);
        IEnumerable<Order> GetOrders(int managerId);
        bool RemoveOrder(int managerId, int orderId);
    }

    /// <summary>
    /// Zbiór funkcji składający się na funkcjonalność kelnera.
    /// </summary>
    public interface IWaiterDataAccess : IBaseDataAccess
    {
        IEnumerable<Order> GetAllPastOrders(int waiterId);
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
        bool WipeUser(int userId);
        bool WipeTable(int tableId);
        bool WipeOrder(int orderId);
    }
}
