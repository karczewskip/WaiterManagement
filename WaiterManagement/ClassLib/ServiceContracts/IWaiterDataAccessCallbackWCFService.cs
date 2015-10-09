using System.ServiceModel;
using ClassLib.DataStructures;

namespace ClassLib.ServiceContracts
{
	public interface IWaiterDataAccessCallbackWCFService
	{
		[OperationContract]
		bool AcceptNewOrder(Order order);
		[OperationContract]
		bool ConfirmUserPaid(int userId);
	}
}