using OrderServiceClient.Abstract;
using OrderServiceClient.WaiterDataAccessWCFService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace OrderServiceClient
{
    class OrderNotyficator : IOrderNotyficator
    {
        private IMainWindowViewModel _mainWindow;
        private CancellationTokenSource _cancelationWaiterResponse;

        public void SetTarget(IMainWindowViewModel mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public bool AcceptNewOrder(Order order)
        {
            return _mainWindow.GetConfirmFromWaiter(order);
        }

        private void WaitForWaiterRespond()
        {
            System.Threading.Thread.Sleep(10000);
        }

        private void NotifyWaiterAboutNewOrder(Order order)
        {
            _mainWindow.ShowNewOrder(order);
        }

        public bool ConfirmUserPaid(int userId)
        {
            MessageBox.Show("Confirmed user paid");
            return true;
        }


        public void AcceptCurrentOrder()
        {
            _cancelationWaiterResponse.Cancel();
        }
    }
}
