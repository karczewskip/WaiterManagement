using System;
using System.Collections.Generic;
using System.Linq;
using ClassLib.DataStructures;
using WebUI.Infrastructure.Abstract;
using WebUI.ClientDataAccessWebWCFService;

namespace WebUI.Infrastructure.Concrete
{
    public class ClientDataAccess : ClientDataAccessWebWCFServiceClient, IClientDataAccess
    {
        public UserContext AddClient(string firstName, string lastName, string login, string password)
        {
            return AddClientWeb(firstName, lastName, login, password);
        }

        public Order AddOrder(int userId, DateTime orderTime, IEnumerable<TupleOfintint> menuItems)
        {
            return AddOrderWeb(userId, orderTime, menuItems.ToArray());
        }

        public IEnumerable<Order> GetOrders(int clientId)
        {
            return GetOrdersClientWeb(clientId);
        }

        public IEnumerable<MenuItemCategory> GetMenuItemCategories(int userId)
        {
            return GetMenuItemCategoriesWeb(userId);
        }

        public IEnumerable<MenuItem> GetMenuItems()
        {
            return GetMenuItemsWeb();
        }

        public IEnumerable<Table> GetTables(int userId)
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