using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderServiceClient.Abstract
{
    interface IMainWindowViewModel
    {
        void LogIn();

        void ShowNewOrder(WaiterDataAccessWCFService.Order order);

        bool GetConfirmFromWaiter(WaiterDataAccessWCFService.Order order);

        void ShowAcceptedOrder(WaiterDataAccessWCFService.Order order);

        bool GetConfirmPayd();

        void CloseCurrentOrder();
    }
}
