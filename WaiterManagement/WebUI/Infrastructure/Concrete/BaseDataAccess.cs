using ClassLib.DataStructures;
using WebUI.ClientDataAccessWebWCFService;
using WebUI.Infrastructure.Abstract;

namespace WebUI.Infrastructure.Concrete
{
    public class BaseDataAccess : ClientDataAccessWebWCFServiceClient, IBaseDataAccess
    {
        public System.Collections.Generic.IEnumerable<MenuItemCategory> GetMenuItemCategories(int userId)
        {
            return GetMenuItemCategoriesWeb(userId);
        }

        public System.Collections.Generic.IEnumerable<MenuItem> GetMenuItems()
        {
            return GetMenuItemsWeb();
        }

        public System.Collections.Generic.IEnumerable<Table> GetTables(int userId)
        {
            return GetTablesWeb(userId);
        }

        public UserContext LogIn(string login, string password)
        {
            return LogInWeb(login, password);
        }

        public bool LogOut(int userId)
        {
            return LogOutWeb(userId);
        }
    }
}