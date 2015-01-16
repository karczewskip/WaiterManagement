using OrderServiceClient.Abstract;
using OrderServiceClient.WaiterDataAccessWCFService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace OrderServiceClient.Service_Communication
{
    class WaiterDataAccess : WaiterDataAccessWCFServiceClient, IWaiterDataAccess
    {
        public WaiterDataAccess(IOrderNotyficator orderNotyficator) :
            base(new InstanceContext(orderNotyficator)) {}
    }
}
