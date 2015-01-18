using OrderClient.Abstract;
using OrderClient.ClientDataAccessWCFService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OrderClient
{
    class OrderNotyficator : IOrderNotyficator
    {
        private IOrderViewModel _orderViewModel;

        public void SetTarget(IOrderViewModel orderViewModel)
        {
            _orderViewModel = orderViewModel;
        }

        public void NotifyOrderAccepted(int orderId, ClientDataAccessWCFService.UserContext waiter)
        {
            _orderViewModel.SetOrderState(OrderState.Accepted);
            MessageBox.Show("Accepted");
        }

        public void NotifyOrderOnHold(int orderId)
        {
            _orderViewModel.SetOrderState(OrderState.Placed);
            MessageBox.Show("On Hold");
        }

        public void NotifyOrderAwaitingDelivery(int oderId)
        {
            MessageBox.Show("AwaitingDelivery");
        }
    }
}
