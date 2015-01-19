using OrderServiceClient.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public bool AcceptNewOrder(WaiterDataAccessWCFService.Order order)
        {
            MessageBox.Show("Accepted order");
            return true;
        }

        public bool ConfirmUserPaid(int userId)
        {
            MessageBox.Show("Confirmed user paid");
            return true;
        }
    }
}
