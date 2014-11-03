using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaiterClient.Abstract;
using DataAccess;
using ClassLib.DbDataStructures;
using System.Windows;

namespace WaiterClient.Model
{
    public class WaiterClientModel : IWaiterClientModel
    {
        private IWaiterDataAccess WaiterDataAccess;

        public WaiterClientModel(IWaiterDataAccess waiterDataAccess)
        {
            WaiterDataAccess = waiterDataAccess;
        }

        public WaiterContext CheckUser(string login, string password)
        {
            try
            {
                return WaiterDataAccess.LogIn(login, password);
            }
            catch
            {
                return null;
            }
        }


        public void LogOut(int waiterId)
        {
            WaiterDataAccess.LogOut(waiterId);
        }

        public IList<Table> GetTables()
        {
            return WaiterDataAccess.GetTables().ToList();
        }

        public IList<MenuItem> GetMenuItems()
        {
            return WaiterDataAccess.GetMenuItems().ToList();
        }

        public IList<MenuItemCategory> GetCategories()
        {
            return WaiterDataAccess.GetMenuItemCategories().ToList();
        }

        public IList<Order> GetActiveOrders(int waiterId)
        {
            return WaiterDataAccess.GetActiveOrders(waiterId).ToList();
        }

        public Order AddNewOrder(int waiterId ,int tableId, IList<MenuItemQuantity> listOfItems )
        {
            var list = new List<Tuple<int,int>>();

            foreach(var i in listOfItems)
            {
                list.Add(new Tuple<int,int>(i.MenuItem.Id, i.Quantity ));
            }

            return WaiterDataAccess.AddOrder(0, tableId, waiterId, list);
        }


        public IList<Order> GetPastOrders(int waiterId, int from, int to)
        {
            var list = WaiterDataAccess.GetPastOrders(waiterId, from, to);

            if (list != null)
            {
                return list.ToList();
            }
            else
                return new List<Order>();
            
        }


        public bool CancelOrder(int waiterId , int orderId)
        {
            return WaiterDataAccess.SetOrderState(waiterId, orderId, OrderState.NotRealized);
        }
    }
}
