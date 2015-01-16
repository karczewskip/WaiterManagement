using OrderServiceClient.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            throw new NotImplementedException();
        }

        public bool ConfirmUserPaid(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
