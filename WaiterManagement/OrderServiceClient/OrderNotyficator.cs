using System;
using OrderServiceClient.Abstract;
using OrderServiceClient.WaiterDataAccessWCFService;

namespace OrderServiceClient
{
    internal class OrderNotyficator : IOrderNotyficator
    {
        private IMainWindowViewModel _mainWindow;

        public void SetTarget(IMainWindowViewModel mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public bool AcceptNewOrder(Order order)
        {
            if (!_mainWindow.GetConfirmFromWaiter(order)) 
                return false;

            _mainWindow.ShowAcceptedOrder(order);
            return true;
        }

        public bool ConfirmUserPaid(int userId)
        {
            if (!_mainWindow.GetConfirmPayd()) 
                return false;
            
            _mainWindow.CloseCurrentOrder();
            return true;
        }

        public void AcceptCurrentOrder()
        {
            throw new NotImplementedException();
        }

        private void NotifyWaiterAboutNewOrder(Order order)
        {
            _mainWindow.ShowNewOrder(order);
        }
    }
}