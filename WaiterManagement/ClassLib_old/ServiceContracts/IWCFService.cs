using ClassLib.DbDataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ClassLib.ServiceContracts
{
    [ServiceContract]
    public interface IWCFService
    {
        [OperationContract]
        WaiterContext LogIn(string login, string pass);

        [OperationContract]
        bool LogOut(WaiterContext waiterContext);

        [OperationContract]
        IEnumerable<MenuItem> GetMenu();

        [OperationContract]
        IEnumerable<MenuItem> GetOrders();

        [OperationContract]
        IEnumerable<Table> GetTables();
    }
}
