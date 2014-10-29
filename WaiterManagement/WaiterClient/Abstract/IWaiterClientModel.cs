using ClassLib.DbDataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaiterClient.Abstract
{
    public interface IWaiterClientModel
    {
        WaiterContext CheckUser(string login, string password);
    }
}
