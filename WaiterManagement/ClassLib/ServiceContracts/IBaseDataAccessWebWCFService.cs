using System.Collections.Generic;
using System.ServiceModel;
using ClassLib.DataStructures;

namespace ClassLib.ServiceContracts
{
	[ServiceContract]
	public interface IBaseDataAccessWebWcfService
	{
		[OperationContract(Name = "GetMenuItemCategoriesWeb")]
		IEnumerable<MenuItemCategory> GetMenuItemCategories(int userId);
		[OperationContract(Name = "GetMenuItemsWeb")]
		IEnumerable<MenuItem> GetMenuItems();
		[OperationContract(Name = "GetTablesWeb")]
		IEnumerable<Table> GetTables(int userId);
		[OperationContract(Name = "LogInWeb")]
		string LogIn(string login, string password);
		[OperationContract(Name = "LogOutWeb")]
		bool LogOut(int userId);
	}
}