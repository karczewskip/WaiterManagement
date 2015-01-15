using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderClient.ClientDataAccessWCFService;

namespace OrderClient.Abstract
{
    interface IOrderNotyficator : IClientDataAccessWCFServiceCallback
    {
        void SetTarget(IOrderViewModel orderViewModel);
    }
}
