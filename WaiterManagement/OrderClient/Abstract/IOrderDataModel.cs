using ClassLib.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderClient.Abstract
{
    interface IOrderDataModel
    {
        Order CurrentOrder { get; set; }

        void AddToCurrentOrder(MenuItem addingMenuItem);

        bool IsEmpty();

        void StartNewOrder();

        void RemoveFromCurrentOrder(MenuItemQuantity removingItem);

        IList<MenuItem> GetAllItems();

        IList<MenuItemCategory> GetAllCategories();

        string GetCurrentOrderMessage();

        void LogIn(string _userName, string password);
    }
}
