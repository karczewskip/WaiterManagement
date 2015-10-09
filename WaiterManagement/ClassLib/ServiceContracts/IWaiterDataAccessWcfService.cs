using System.Collections.Generic;
using System.ServiceModel;
using ClassLib.DataStructures;

namespace ClassLib.ServiceContracts
{
	[ServiceContract(CallbackContract = typeof(IWaiterDataAccessCallbackWCFService))]
	public interface IWaiterDataAccessWcfService : IBaseDataAccessWcfService
	{
		[OperationContract]
		IEnumerable<Order> GetAllPastOrders(int waiterId);
		[OperationContract]
		IEnumerable<Order> GetPastOrders(int waiterId, int firstIndex, int lastIndex);
		[OperationContract]
		IEnumerable<Order> GetActiveOrders(int waiterId);
		[OperationContract]
		bool SetOrderState(int waiterId, int orderId, OrderState state);
	}
}