using System.Collections.Generic;
using ClassLib.DataStructures;
using WebUI.Infrastructure.Abstract;
using WebUI.ManagerDataAccessWebWCFService;

namespace WebUI.Infrastructure.Concrete
{
    public class ManagerDataAccess : ManagerDataAccessWebWCFServiceClient, IManagerDataAccess
    {
        public UserContext AddManager(string firstName, string lastName, string login, string password)
        {
            return AddManager(firstName, lastName, login, password);
        }

        public MenuItemCategory AddMenuItemCategory(int managerId, string name, string description)
        {
            return AddMenuItemCategory(managerId, name, description);
        }

        public bool EditMenuItemCategory(int managerId, MenuItemCategory menuItemCategoryToEdit)
        {
            return EditMenuItemCategoryWeb(managerId, menuItemCategoryToEdit);
        }

        public bool RemoveMenuItemCategory(int managerId, int categoryId)
        {
            return RemoveMenuItemCategoryWeb(managerId, categoryId);
        }

        public MenuItem AddMenuItem(int managerId, string name, string description, int categoryId, Money price)
        {
            return AddMenuItem(managerId, name, description, categoryId, price);
        }

        public bool EditMenuItem(int managerId, MenuItem menuItemToEdit)
        {
            return EditMenuItemWeb(managerId, menuItemToEdit);
        }

        public bool RemoveMenuItem(int managerId, int menuItemId)
        {
            return RemoveMenuItemWeb(managerId, menuItemId);
        }

        public UserContext AddWaiter(int managerId, string firstName, string lastName, string login, string password)
        {
            return AddWaiterWeb(managerId, firstName, lastName, login, password);
        }

        public bool EditWaiter(int managerId, UserContext waiterToEdit)
        {
            return EditWaiterWeb(managerId, waiterToEdit);
        }

        public bool RemoveWaiter(int managerId, int waiterId)
        {
            return RemoveWaiterWeb(managerId, waiterId);
        }

        public IEnumerable<UserContext> GetWaiters(int managerId)
        {
            return GetWaitersWeb(managerId);
        }

        public Table AddTable(int managerId, int tableNumber, string description)
        {
            return AddTable(managerId, tableNumber, description);
        }

        public bool EditTable(int managerId, Table tableToEdit)
        {
            return EditTable(managerId, tableToEdit);
        }

        public bool RemoveTable(int managerId, int tableId)
        {
            return RemoveTable(managerId, tableId);
        }

        public IEnumerable<Order> GetOrders(int managerId)
        {
            return GetOrdersWeb(managerId);
        }

        public bool RemoveOrder(int managerId, int orderId)
        {
            return RemoveOrderWeb(managerId, orderId);
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
            return GetTables(userId);
        }

        public UserContext LogIn(string login, string password)
        {
            return LogInWeb(login, password);
        }

        public bool LogOut(int userId)
        {
            return LogOut(userId);
        }
    }
}