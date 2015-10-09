using System.Collections.Generic;
using System.ServiceModel;
using ClassLib.DataStructures;

namespace ClassLib.ServiceContracts
{
	[ServiceContract]
	public interface IBaseDataAccessWcfService
	{
		[OperationContract]
		string LogIn(string login, string password);
		[OperationContract]
		bool LogOut(int id);
		[OperationContract]
		IEnumerable<MenuItemCategory> GetMenuItemCategories(int userId);
		[OperationContract]
		IEnumerable<MenuItem> GetMenuItems(int userId);
		[OperationContract]
		IEnumerable<Table> GetTables(int userId);
	}
}