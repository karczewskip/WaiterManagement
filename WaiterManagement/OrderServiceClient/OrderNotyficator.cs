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

        public void SetTarget(IMainWindowViewModel mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public bool AcceptNewOrder(Order order)
        {
            if(_mainWindow.GetConfirmFromWaiter(order))
            {
                _mainWindow.ShowAcceptedOrder(order);
                return true;
            }

            return false;
        }

        private void NotifyWaiterAboutNewOrder(Order order)
        {
            _mainWindow.ShowNewOrder(order);
        }

        public bool ConfirmUserPaid(int userId)
        {
            //if(_mainWindow.GetConfirmPayd(order))
            //{
            //    //_mainWindow.Close
            //    return true;
            //}
            return true;
        }


        public void AcceptCurrentOrder()
        {
            throw new NotImplementedException();
        }
    }
}
