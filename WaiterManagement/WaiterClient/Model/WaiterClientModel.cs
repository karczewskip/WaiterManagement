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
    /// <summary>
    /// Klasa odpowiedzialna za dostarczanie i przetwarzanie danych z bazy danych
    /// </summary>
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
            try
            {
                WaiterDataAccess.LogOut(waiterId);
            }
            catch
            {
                throw new Exception("Problem with DB");
            }
        }

        public IList<Table> GetTables()
        {
            try
            {
                return WaiterDataAccess.GetTables().ToList();
            }
            catch
            {
                throw new Exception("Problem with DB");
            }
        }

        public IList<MenuItem> GetMenuItems()
        {
            try
            {
                return WaiterDataAccess.GetMenuItems().ToList();
            }
            catch
            {
                throw new Exception("Problem with DB");
            }
        }

        public IList<MenuItemCategory> GetCategories()
        {
            try
            {
                return WaiterDataAccess.GetMenuItemCategories().ToList();
            }
            catch
            {
                throw new Exception("Problem with DB");
            }
        }

        public IList<Order> GetActiveOrders(int waiterId)
        {
            try
            {
                return WaiterDataAccess.GetActiveOrders(waiterId).ToList();
            }
            catch
            {
                throw new Exception("Problem with DB");
            }
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

        public bool RelizeOrder(int waiterId, int orderId)
        {
            return WaiterDataAccess.SetOrderState(waiterId, orderId, OrderState.Realized);
        }
    }
}
