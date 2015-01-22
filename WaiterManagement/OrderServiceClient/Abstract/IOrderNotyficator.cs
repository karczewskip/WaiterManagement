using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderServiceClient.WaiterDataAccessWCFService;

namespace OrderServiceClient.Abstract
{
    interface IOrderNotyficator: IWaiterDataAccessWCFServiceCallback
    {
        void SetTarget(IMainWindowViewModel mainWindow);

        void AcceptCurrentOrder();
    }
}
