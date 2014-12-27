using ClassLib.DbDataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib.ServiceContracts
{
    public interface IBaseDataAccessWCFService
    {
        [OperationContract]
        UserContext LogIn(string login, string password);
        [OperationContract]
        bool LogOut(int id);
        [OperationContract]
        IEnumerable<MenuItemCategory> GetMenuItemCategories();
        [OperationContract]
        IEnumerable<MenuItem> GetMenuItems();
        [OperationContract]
        IEnumerable<Table> GetTables();
    }

    public interface IManagerDataAccessWCFService : IBaseDataAccessWCFService
    {
        [OperationContract]
        MenuItemCategory AddMenuItemCategory(string name, string description);
        [OperationContract]
        bool EditMenuItemCategory(MenuItemCategory menuItemCategoryToEdit);
        [OperationContract]
        bool RemoveMenuItemCategory(int categoryId);
        [OperationContract]
        MenuItem AddMenuItem(string name, string description, int categoryId, Money price);
        [OperationContract]
        bool EditMenuItem(MenuItem menuItemToEdit);
        [OperationContract]
        bool RemoveMenuItem(int menuItemId);
        [OperationContract]
        UserContext AddWaiter(string firstName, string lastName, string login, string password);
        [OperationContract]
        bool EditWaiter(UserContext waiterToEdit);
        [OperationContract]
        bool RemoveWaiter(int waiterId);
        [OperationContract]
        IEnumerable<UserContext> GetWaiters();
        [OperationContract]
        Table AddTable(int tableNumber, string description);
        [OperationContract]
        bool EditTable(Table tableToEdit);
        [OperationContract]
        bool RemoveTable(int tableId);
        [OperationContract]
        IEnumerable<Order> GetOrders();
        [OperationContract]
        bool RemoveOrder(int orderId);
    }

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

    public  interface IClientDataAccessWCFService : IBaseDataAccessWCFService
    {
        [OperationContract]
        Order AddOrder(int userId, int tableId, int waiterId, IEnumerable<Tuple<int, int>> menuItems);
    }
}
