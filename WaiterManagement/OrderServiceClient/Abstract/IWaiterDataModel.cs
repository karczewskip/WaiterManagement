using OrderServiceClient.WaiterDataAccessWCFService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderServiceClient.Abstract
{
    interface IWaiterDataModel
    {
        void LogIn(string _userName, string password);

        bool IsLogged();
    }
}
