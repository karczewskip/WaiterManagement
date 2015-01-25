using ClassLib.DataStructures;
using OrderServiceClient.Abstract;
using Order = OrderServiceClient.WaiterDataAccessWCFService.Order;
using OrderState = OrderServiceClient.WaiterDataAccessWCFService.OrderState;
using UserContext = OrderServiceClient.WaiterDataAccessWCFService.UserContext;

namespace OrderServiceClient.Model
{
    internal class WaiterDataModel : IWaiterDataModel
    {
        private readonly IOrderNotyficator _orderNotyficator;
        private readonly IWaiterDataAccess _waiterDataAccess;
        private UserContext waiterUserContext;

        public WaiterDataModel(IWaiterDataAccess waiterDataAccess, IOrderNotyficator orderNotyficator)
        {
            _waiterDataAccess = waiterDataAccess;
            _orderNotyficator = orderNotyficator;
            waiterUserContext = null;
        }

        public void LogIn(string login, string password)
        {
            waiterUserContext = _waiterDataAccess.LogIn(login, HashClass.CreateFirstHash(password, login));
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