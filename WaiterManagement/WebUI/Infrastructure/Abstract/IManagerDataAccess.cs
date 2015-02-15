using System.Collections.Generic;
using System.ServiceModel;
using ClassLib.DataStructures;

namespace WebUI.Infrastructure.Abstract
{
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
}
