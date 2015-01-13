using System.Collections.Generic;
using WaiterClient.WaiterDataAccessWCFService;

namespace WaiterClient.Abstract
{
    public interface IWaiterClientModel
    {
        bool LogInUser(string login, string password);

        bool LogOut();

        IList<Table> GetTables();

        IList<MenuItem> GetMenuItems();

        IList<MenuItemCategory> GetCategories();

        IList<Order> GetActiveOrders();

        IList<Order> GetPastOrders(int from, int to);

        bool CancelOrder(int orderId);

        bool RealizeOrder(int p);
    }
}
