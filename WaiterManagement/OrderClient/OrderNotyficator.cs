using OrderClient.Abstract;
using OrderClient.ClientDataAccessWCFService;

namespace OrderClient
{
    internal class OrderNotyficator : IOrderNotyficator
    {
        private IOrderViewModel _orderViewModel;

        public void SetTarget(IOrderViewModel orderViewModel)
        {
            _orderViewModel = orderViewModel;
        }

        public void NotifyOrderAccepted(int orderId, UserContext waiter)
        {
            _orderViewModel.SetOrderState(OrderState.Accepted);
        }

        public void NotifyOrderOnHold(int orderId)
        {
            _orderViewModel.NotyfyOrderOnHold();
        }

        public void NotifyOrderAwaitingDelivery(int oderId)
        {
            _orderViewModel.ShowPayingWindow();
        }
    }
}