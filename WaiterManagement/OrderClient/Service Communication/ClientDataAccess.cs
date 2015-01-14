using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderClient.ClientDataAccessWCFService;
using OrderClient.Abstract;

namespace OrderClient.Service_Communication
{
    class ClientDataAccess : ClientDataAccessWCFServiceClient, IClientDataAccess
    {
    }
}
