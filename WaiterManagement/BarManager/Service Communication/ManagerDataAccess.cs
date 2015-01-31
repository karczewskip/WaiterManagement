using System;
using BarManager.Abstract;
using BarManager.Abstract.Model;
using BarManager.ManagerDataAccessWCFService;

namespace BarManager.Service_Communication
{
    class ManagerDataAccess : ManagerDataAccessWCFServiceClient, IManagerDataAccess, IDisposable
    {
        private ICredentialDataModel _credentialDataModel;

        public void Dispose()
        {
            Close();
        }

        private int GetId()
        {
            return _credentialDataModel.UserContext.Id;
        }

        public bool RemoveTable(int id)
        {
            return RemoveTable(GetId(), id);
        }

        public Table[] GetTables()
        {
            return GetTables(GetId());
        }

        public Table AddTable(int number, string tableDescription)
        {
            return AddTable(GetId(), number, tableDescription);
        }

        public bool EditTable(Table table)
        {
            return EditTable(GetId(), table);
        }

        public UserContext[] GetWaiters()
        {
            return GetWaiters(GetId());
        }

        public bool RemoveWaiter(int id)
        {
            return RemoveWaiter(GetId(), id);
        }

        public UserContext AddWaiter(string firstName, string lastName, string login, string password)
        {
            return AddWaiter(GetId(), firstName, lastName, login, password);
        }

        public bool EditWaiter(UserContext waiter)
        {
            return EditWaiter(GetId(), waiter);
        }

        public MenuItem AddMenuItem(string menuItemName, string menuItemDescription, int categoryId, Money money)
        {
            return AddMenuItem(GetId(), menuItemName, menuItemDescription, categoryId, money);
        }

        public bool RemoveMenuItem(int id)
        {
            return RemoveMenuItem(GetId(), id);
        }

        public bool EditMenuItem(MenuItem menuItemToEdit)
        {
            return EditMenuItem(GetId(), menuItemToEdit);
        }

        public MenuItem[] GetMenuItems()
        {
            return GetMenuItems(GetId());
        }

        public MenuItemCategory AddMenuItemCategory(string categoryName, string categoryDescription)
        {
            return AddMenuItemCategory(GetId(), categoryName, categoryDescription);
        }

        public MenuItemCategory[] GetMenuItemCategories()
        {
            return GetMenuItemCategories(GetId());
        }

        public void SetCredentialDataModel(ICredentialDataModel credentialDataModel)
        {
            _credentialDataModel = credentialDataModel;
        }
    }
}
