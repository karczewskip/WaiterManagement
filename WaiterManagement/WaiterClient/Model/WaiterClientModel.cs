using ClassLib.DbDataStructures;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using WaiterClient.Abstract;

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

        /// <summary>
        /// Login waiter
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public UserContext LogInUser(string login, string password)
        {
            try
            {
                return WaiterDataAccess.LogIn(login, password);
            }
            catch
            {
                throw new Exception("Problem with DB");
            }
        }

        /// <summary>
        /// Logout waiter
        /// </summary>
        /// <param name="waiterId"></param>
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

        /// <summary>
        /// Get all tables
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Get all menu items
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Get all categories
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Get all active orders current waiter
        /// </summary>
        /// <param name="waiterId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Add new order
        /// </summary>
        /// <param name="waiterId"></param>
        /// <param name="tableId"></param>
        /// <param name="listOfItems"></param>
        /// <returns></returns>
        public Order AddNewOrder(int waiterId ,int tableId, IList<MenuItemQuantity> listOfItems )
        {
            var list = listOfItems.Select(i => new Tuple<int, int>(i.MenuItem.Id, i.Quantity)).ToList();

            try
            {
                return WaiterDataAccess.AddOrder(0, tableId, waiterId, list);
            }
            catch 
            {

                throw new Exception("Problem with DB");
            }
            
        }

        /// <summary>
        /// Get relized and rejected orders
        /// </summary>
        /// <param name="waiterId"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public IList<Order> GetPastOrders(int waiterId, int from, int to)
        {
            IEnumerable<Order> list;
            try
            {
                list = WaiterDataAccess.GetPastOrders(waiterId, from, to);
            }
            catch (Exception)
            {
                throw new Exception("Problem with DB");
            }
            

            if (list != null)
            {
                return list.ToList();
            }
            else
                return new List<Order>();
           
        }

        /// <summary>
        /// Cancel order
        /// </summary>
        /// <param name="waiterId"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public bool CancelOrder(int waiterId , int orderId)
        {
            return WaiterDataAccess.SetOrderState(waiterId, orderId, OrderState.NotRealized);
        }

        /// <summary>
        /// Relize Order
        /// </summary>
        /// <param name="waiterId"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public bool RelizeOrder(int waiterId, int orderId)
        {
            return WaiterDataAccess.SetOrderState(waiterId, orderId, OrderState.Realized);
        }
    }
}
