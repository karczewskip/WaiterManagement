using System.ServiceModel;
using BarManager.Abstract.Model;
using BarManager.ManagerDataAccessWCFService;
using BarManager.Model;

namespace BarManager.Abstract
{
    public interface IManagerDataAccess : ICommunicationObject, IManagerDataAccessWCFService
    {
        bool RemoveTable(int id);

        Table[] GetTables();

        Table AddTable(int number, string tableDescription);

        bool EditTable(Table table);

        UserContext[] GetWaiters();

        bool RemoveWaiter(int id);

        UserContext AddWaiter(string firstName, string lastName, string login, string p);

        bool EditWaiter(UserContext waiter);

        MenuItem AddMenuItem(string menuItemName, string menuItemDescription, int p, Money money);

        bool RemoveMenuItem(int id);

        bool EditMenuItem(MenuItem menuItemToEdit);

        MenuItem[] GetMenuItems();

        MenuItemCategory AddMenuItemCategory(string categoryName, string categoryDescription);

        MenuItemCategory[] GetMenuItemCategories();

        void SetCredentialDataModel(ICredentialDataModel credentialDataModel);
    }
}