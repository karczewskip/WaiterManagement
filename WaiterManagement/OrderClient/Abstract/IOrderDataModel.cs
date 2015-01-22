using OrderClient.ClientDataAccessWCFService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderClient.Abstract
{
    interface IOrderDataModel
    {
        void AddToCurrentOrder(MenuItem addingMenuItem);

        bool IsEmpty();

        void StartNewOrder();

        void RemoveFromCurrentOrder(MenuItemQuantity removingItem);

        IList<MenuItem> GetAllItems();

        IList<MenuItemCategory> GetAllCategories();

        string GetCurrentOrderMessage();

        void SetTargetMessage(IOrderViewModel orderViewModel);

        void AddClient(string firstName, string lastName, string login, string password);

        IList<MenuItemQuantity> MenuItems { get; set; }

        void AddOrder();

        IList<Table> GetTables();

        void SetTableId(int p);

        void SetOrderState(OrderState state);

        void Login(string _userName, string p);

        void Pay();

        bool IsLogged();
    }
}
