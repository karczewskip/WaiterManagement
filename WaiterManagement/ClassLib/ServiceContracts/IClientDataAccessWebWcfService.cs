using System;
using System.Collections.Generic;
using System.ServiceModel;
using ClassLib.DataStructures;

namespace ClassLib.ServiceContracts
{
	[ServiceContract]
    public interface IClientDataAccessWebWcfService : IBaseDataAccessWebWcfService
    {
        [OperationContract(Name = "AddClientWeb")]
        UserContext AddClient(string firstName, string lastName, string login, string password);
        [OperationContract(Name = "AddOrderWeb")]
        Order AddOrder(int userId, DateTime orderTime, IEnumerable<Tuple<int, int>> menuItems);
        [OperationContract(Name = "GetOrdersClientWeb")]
        IEnumerable<Order> GetOrders(int clientId);
    }
}
