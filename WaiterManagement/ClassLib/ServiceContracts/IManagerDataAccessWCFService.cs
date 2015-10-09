using System.Collections.Generic;
using System.ServiceModel;
using ClassLib.DataStructures;

namespace ClassLib.ServiceContracts
{
	[ServiceContract]
	public interface IManagerDataAccessWcfService : IBaseDataAccessWcfService
	{
		[OperationContract]
		void AddManager(string firstName, string lastName, string login, string password);
		[OperationContract]
		MenuItemCategory AddMenuItemCategory(int managerId, string name, string description);
		[OperationContract]
		bool EditMenuItemCategory(int managerId, MenuItemCategory menuItemCategoryToEdit);
		[OperationContract]
		bool RemoveMenuItemCategory(int managerId, int categoryId);
		[OperationContract]
		MenuItem AddMenuItem(int managerId, string name, string description, int categoryId, Money price);
		[OperationContract]
		bool EditMenuItem(int managerId, MenuItem menuItemToEdit);
		[OperationContract]
		bool RemoveMenuItem(int managerId, int menuItemId);
		[OperationContract]
		UserContext AddWaiter(int managerId, string firstName, string lastName, string login, string password);
		[OperationContract]
		bool EditWaiter(int managerId, UserContext waiterToEdit);
		[OperationContract]
		bool RemoveWaiter(int managerId, int waiterId);
		[OperationContract]
		IEnumerable<UserContext> GetWaiters(int managerId);
		[OperationContract]
		Table AddTable(int managerId, int tableNumber, string description);
		[OperationContract]
		bool EditTable(int managerId, Table tableToEdit);
		[OperationContract]
		bool RemoveTable(int managerId, int tableId);
		[OperationContract]
		IEnumerable<Order> GetOrders(int managerId);
		[OperationContract]
		bool RemoveOrder(int managerId, int orderId);
	}
}