using System.Collections.Generic;
using System.ServiceModel;
using ClassLib.DataStructures;

namespace WebUI.Infrastructure.Abstract
{
    public interface IBaseDataAccess : ICommunicationObject
    {
        IEnumerable<MenuItemCategory> GetMenuItemCategories(int userId);
        IEnumerable<MenuItem> GetMenuItems();
        IEnumerable<Table> GetTables(int userId);
        UserContext LogIn(string login, string password);
        bool LogOut(int userId);
    }
}
