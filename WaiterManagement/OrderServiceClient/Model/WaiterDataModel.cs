using OrderServiceClient.Abstract;
using OrderServiceClient.WaiterDataAccessWCFService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderServiceClient.Model
{
    class WaiterDataModel : IWaiterDataModel
    {
        private IWaiterDataAccess _waiterDataAccess;
        private IOrderNotyficator _orderNotyficator;
        private UserContext waiterUserContext;

        public WaiterDataModel(IWaiterDataAccess waiterDataAccess, IOrderNotyficator orderNotyficator)
        {
            _waiterDataAccess = waiterDataAccess;
            _orderNotyficator = orderNotyficator;
            waiterUserContext = null;
        }

        public void LogIn(string login, string password)
        {
            waiterUserContext = _waiterDataAccess.LogIn(login, ClassLib.DataStructures.HashClass.CreateFirstHash(password, login));
        }

        public bool IsLogged()
        {
            return waiterUserContext != null;
        }


        public void AcceptOrder()
        {
            _orderNotyficator.AcceptCurrentOrder();
        }


        public void SetOrderAwaiting(Order order)
        {
            _waiterDataAccess.SetOrderState(waiterUserContext.Id, order.Id, OrderState.Accepted);
        }

        public void NotifyReady(Order order)
        {
            _waiterDataAccess.SetOrderState(waiterUserContext.Id, order.Id, OrderState.AwaitingDelivery);
        }
    }
}
