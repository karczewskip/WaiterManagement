using System;
using System.Collections.Generic;
using System.ServiceModel;
using ClassLib.DataStructures;

namespace ClassLib.ServiceContracts
{
	[ServiceContract(CallbackContract = typeof(IClientDataAccessCallbackWCFService))]
	public interface IClientDataAccessWcfService : IBaseDataAccessWcfService
	{
		[OperationContract]
		UserContext AddClient(string firstName, string lastName, string login, string password);
		[OperationContract]
		Order AddOrder(int userId, int tableId, IEnumerable<Tuple<int, int>> menuItems);
		[OperationContract]
		bool PayForOrder(int userId, int orderId);
	}
}