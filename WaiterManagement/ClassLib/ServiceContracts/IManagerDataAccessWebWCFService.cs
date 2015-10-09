using System.Collections.Generic;
using System.ServiceModel;
using ClassLib.DataStructures;

namespace ClassLib.ServiceContracts
{
	[ServiceContract]
	public interface IManagerDataAccessWebWCFService : IBaseDataAccessWebWcfService
	{
		[OperationContract(Name = "AddManagerWeb")]
		void AddManager(string firstName, string lastName, string login, string password);
		[OperationContract(Name = "AddMenuItemCategoryWeb")]
		MenuItemCategory AddMenuItemCategory(int managerId, string name, string description);
		[OperationContract(Name = "EditMenuItemCategoryWeb")]
		bool EditMenuItemCategory(int managerId, MenuItemCategory menuItemCategoryToEdit);
		[OperationContract(Name = "RemoveMenuItemCategoryWeb")]
		bool RemoveMenuItemCategory(int managerId, int categoryId);
		[OperationContract(Name = "AddMenuItemWeb")]
		MenuItem AddMenuItem(int managerId, string name, string description, int categoryId, Money price);
		[OperationContract(Name = "EditMenuItemWeb")]
		bool EditMenuItem(int managerId, MenuItem menuItemToEdit);
		[OperationContract(Name = "RemoveMenuItemWeb")]
		bool RemoveMenuItem(int managerId, int menuItemId);
		[OperationContract(Name = "AddWaiterWeb")]
		UserContext AddWaiter(int managerId, string firstName, string lastName, string login, string password);
		[OperationContract(Name = "EditWaiterWeb")]
		bool EditWaiter(int managerId, UserContext waiterToEdit);
		[OperationContract(Name = "RemoveWaiterWeb")]
		bool RemoveWaiter(int managerId, int waiterId);
		[OperationContract(Name = "GetWaitersWeb")]
		IEnumerable<UserContext> GetWaiters(int managerId);
		[OperationContract(Name = "AddTableWeb")]
		Table AddTable(int managerId, int tableNumber, string description);
		[OperationContract(Name = "EditTableWeb")]
		bool EditTable(int managerId, Table tableToEdit);
		[OperationContract(Name = "RemoveTableWeb")]
		bool RemoveTable(int managerId, int tableId);
		[OperationContract(Name = "GetOrdersWeb")]
		IEnumerable<Order> GetOrders(int managerId);
		[OperationContract(Name = "RemoveOrderWeb")]
		bool RemoveOrder(int managerId, int orderId);
	}
}