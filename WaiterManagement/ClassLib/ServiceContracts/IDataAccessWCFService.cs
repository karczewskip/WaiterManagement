using ClassLib.DbDataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib.ServiceContracts
{
    [ServiceContract]
    public interface IBaseDataAccessWCFService
    {
        [OperationContract]
        UserContext LogIn(string login, string password);
        [OperationContract]
        bool LogOut(int id);
        [OperationContract]
        IEnumerable<MenuItemCategory> GetMenuItemCategories(int userId);
        [OperationContract]
        IEnumerable<MenuItem> GetMenuItems(int userId);
        [OperationContract]
        IEnumerable<Table> GetTables(int userId);
    }

    [ServiceContract]
    public interface IManagerDataAccessWCFService : IBaseDataAccessWCFService
    {
        [OperationContract]
        UserContext AddManager(string firstName, string lastName, string login, string lastName);
        [OperationContract]
        MenuItemCategory AddMenuItemCategory(int managerId, string name, string description);
        [OperationContract]
        bool EditMenuItemCategory(int managerId, MenuItemCategory menuItemCategoryToEdit);
        [OperationContract]
        bool RemoveMenuItemCategory(int managerId, int categoryId);
        [OperationContract]
        MenuItem AddMenuItem(int managerId, string name, string description, int categoryId, Money price);
        [OperationContract]
        bool EditMenuItem(int managerId, MenuItem menuItemToEdit);
        [OperationContract]
        bool RemoveMenuItem(int managerId, int menuItemId);
        [OperationContract]
        UserContext AddWaiter(int managerId, string firstName, string lastName, string login, string password);
        [OperationContract]
        bool EditWaiter(int managerId, UserContext waiterToEdit);
        [OperationContract]
        bool RemoveWaiter(int managerId, int waiterId);
        [OperationContract]
        IEnumerable<UserContext> GetWaiters(int managerId);
        [OperationContract]
        Table AddTable(int managerId, int tableNumber, string description);
        [OperationContract]
        bool EditTable(int managerId, Table tableToEdit);
        [OperationContract]
        bool RemoveTable(int managerId, int tableId);
        [OperationContract]
        IEnumerable<Order> GetOrders(int managerId);
        [OperationContract]
        bool RemoveOrder(int managerId, int orderId);
    }

    [ServiceContract]
    public interface IWaiterDataAccessWCFService : IBaseDataAccessWCFService
    {
        [OperationContract]
        IEnumerable<Order> GetPastOrders(int waiterId);
        [OperationContract]
        IEnumerable<Order> GetPastOrders(int waiterId, int firstIndex, int lastIndex);
        [OperationContract]
        IEnumerable<Order> GetActiveOrders(int waiterId);
        [OperationContract]
        bool SetOrderState(int waiterId, int orderId, OrderState state);
    }

    [ServiceContract]
    public  interface IClientDataAccessWCFService : IBaseDataAccessWCFService
    {
        [OperationContract]
        Order AddOrder(int userId, int tableId, int waiterId, IEnumerable<Tuple<int, int>> menuItems);
    }
}
