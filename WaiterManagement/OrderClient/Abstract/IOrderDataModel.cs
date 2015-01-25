using System.Collections.Generic;
using OrderClient.ClientDataAccessWCFService;

namespace OrderClient.Abstract
{
    public interface IOrderDataModel
    {
        IList<MenuItemQuantity> MenuItems { get; set; }
        void AddToCurrentOrder(MenuItem addingMenuItem);
        bool IsEmpty();
        void StartNewOrder();
        void RemoveFromCurrentOrder(MenuItemQuantity removingItem);
        IList<MenuItem> GetAllItems();
        IList<MenuItemCategory> GetAllCategories();
        string GetCurrentOrderMessage();
        void SetTargetMessage(IOrderViewModel orderViewModel);
        void AddClient(string firstName, string lastName, string login, string password);
        void AddOrder();
        IList<Table> GetTables();
        void SetTableId(int p);
        void SetOrderState(OrderState state);
        void Login(string _userName, string p);
        void Pay();
        bool IsLogged();

        void SetCurrentOrderOnHold();

        void LogOut();
    }
}