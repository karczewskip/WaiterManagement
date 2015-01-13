using System;
using System.Collections.Generic;
using System.Linq;
using WaiterClient.Abstract;
using WaiterClient.WaiterDataAccessWCFService;

namespace WaiterClient.Model
{
    /// <summary>
    /// Klasa odpowiedzialna za dostarczanie i przetwarzanie danych z bazy danych
    /// </summary>
    public class WaiterClientModel : IWaiterClientModel, IDisposable
    {
        private WaiterDataAccessWCFServiceClient WaiterDataAccess;
        private UserContext waiterUserContext = null;

        public WaiterClientModel(/*IWaiterDataAccess waiterDataAccess*/)
        {
            WaiterDataAccess = new WaiterDataAccessWCFServiceClient();
        }

        /// <summary>
        /// Login waiter
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool LogInUser(string login, string password)
        {
            try
            {
               waiterUserContext = WaiterDataAccess.LogIn(login, ClassLib.DataStructures.HashClass.CreateFirstHash(password, login));
            }
            catch
            {
                throw new Exception("Problem with DB");
            }
            return waiterUserContext != null;
        }

        /// <summary>
        /// Logout waiter
        /// </summary>
        public bool LogOut()
        {
            if (waiterUserContext == null)
                return false;

            try
            {
                return WaiterDataAccess.LogOut(waiterUserContext.Id);
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
            if (waiterUserContext == null)
                return null;

            try
            {
                return WaiterDataAccess.GetTables(waiterUserContext.Id).ToList();
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
            if (waiterUserContext == null)
                return null;

            try
            {
                return WaiterDataAccess.GetMenuItems(waiterUserContext.Id).ToList();
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
            if (waiterUserContext == null)
                return null;

            try
            {
                return WaiterDataAccess.GetMenuItemCategories(waiterUserContext.Id).ToList();
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
        public IList<Order> GetActiveOrders()
        {
            if (waiterUserContext == null)
                return null;

            try
            {
                return WaiterDataAccess.GetActiveOrders(waiterUserContext.Id).ToList();
            }
            catch
            {
                throw new Exception("Problem with DB");
            }
        }

        /// <summary>
        /// Get relized and rejected orders
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public IList<Order> GetPastOrders(int from, int to)
        {
            if (waiterUserContext == null)
                return null;

            IList<Order> list = null;
            try
            {
                list = WaiterDataAccess.GetPastOrders(waiterUserContext.Id, from, to).ToList();
            }
            catch (Exception)
            {
                throw new Exception("Problem with DB");
            }

            return list;
        }

        /// <summary>
        /// Cancel order
        /// </summary>
        /// <param name="waiterId"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public bool CancelOrder(int orderId)
        {
            if (waiterUserContext == null)
                return false;

            return WaiterDataAccess.SetOrderState(waiterUserContext.Id, orderId, OrderState.NotRealized);
        }

        /// <summary>
        /// Relize Order
        /// </summary>
        /// <param name="waiterId"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public bool RealizeOrder(int orderId)
        {
            if (waiterUserContext == null)
                return false;

            return WaiterDataAccess.SetOrderState(waiterUserContext.Id, orderId, OrderState.Realized);
        }

        //Klasa implementuje interfejs IDisposable, aby móc bezpiecznie zamknąć połączenie z usługą WCF-ową
        public void Dispose()
        {
            WaiterDataAccess.Close();
        }
    }
}
