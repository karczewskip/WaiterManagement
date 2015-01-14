using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace OrderServiceClient.Abstract
{
    interface IWaiterDataAccess : ICommunicationObject, OrderServiceClient.WaiterDataAccessWCFService.IWaiterDataAccessWCFService
    {
    }
}
