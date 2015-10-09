using System.ServiceModel;
using ClassLib.DataStructures;

namespace ClassLib.ServiceContracts
{
	public interface IClientDataAccessCallbackWCFService
	{
		[OperationContract]
		void NotifyOrderAccepted(int orderId, UserContext waiter);
		[OperationContract]
		void NotifyOrderOnHold(int orderId);
		[OperationContract]
		void NotifyOrderAwaitingDelivery(int oderId);
	}
}