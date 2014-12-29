using ClassLib.DbDataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaiterClient.Abstract
{
    public interface IWaiterClientModel
    {
        UserContext LogInUser(string login, string password);

        void LogOut(int WaiterId);

        IList<Table> GetTables();

        IList<MenuItem> GetMenuItems();

        IList<MenuItemCategory> GetCategories();

        IList<Order> GetActiveOrders(int waiterId);

        Order AddNewOrder(int waiterId, int tableId, IList<MenuItemQuantity> listOfItems);


        IList<Order> GetPastOrders(int waiterId, int from, int to);

        bool CancelOrder(int waiterId, int orderId);

        bool RelizeOrder(int WaiterId, int p);
    }
}
