using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ClassLib.DataStructures;
using OrderClient.Abstract;
using OrderClient.ClientDataAccessWCFService;
using MenuItem = OrderClient.ClientDataAccessWCFService.MenuItem;
using MenuItemCategory = OrderClient.ClientDataAccessWCFService.MenuItemCategory;
using MenuItemQuantity = OrderClient.ClientDataAccessWCFService.MenuItemQuantity;
using Order = OrderClient.ClientDataAccessWCFService.Order;
using OrderState = OrderClient.ClientDataAccessWCFService.OrderState;
using Table = OrderClient.ClientDataAccessWCFService.Table;
using UserContext = OrderClient.ClientDataAccessWCFService.UserContext;

namespace OrderClient.Model
{
    internal class OrderDataModel : IOrderDataModel
    {
        private readonly IClientDataAccess _clientDataAccess;
        private readonly IOrderNotyficator _orderNotyficator;

        private bool _isCurrentOrderOnHold;
        private Order _currentOrder;
        private int _tableId;
        private UserContext _userContext;

        public OrderDataModel(IClientDataAccess clientDataAccess, IOrderNotyficator orderNotyficator)
        {
            _clientDataAccess = clientDataAccess;
            _orderNotyficator = orderNotyficator;

            MenuItems = new List<MenuItemQuantity>();
        }

        private OrderState CurrentOrderState { get; set; }
        public IList<MenuItemQuantity> MenuItems { get; set; }

        public void AddToCurrentOrder(MenuItem addingMenuItem)
        {
            var typeOfAddingOrder = FindThisTypeOfOrder(addingMenuItem);

            if (typeOfAddingOrder == null)
            {
                MenuItems.Add(new MenuItemQuantity {MenuItem = addingMenuItem, Quantity = 1});
            }
            else
            {
                typeOfAddingOrder.Quantity++;
            }
        }

        public bool IsEmpty()
        {
            return MenuItems.Count == 0;
        }

        public void StartNewOrder()
        {
            MenuItems = new List<MenuItemQuantity>();
            _isCurrentOrderOnHold = false;
        }

        public void RemoveFromCurrentOrder(MenuItemQuantity removingItem)
        {
            MenuItems.Remove(removingItem);
        }

        public IList<MenuItem> GetAllItems()
        {
            return _clientDataAccess.GetMenuItems(_userContext.Id).ToList();
        }

        public IList<MenuItemCategory> GetAllCategories()
        {
            return _clientDataAccess.GetMenuItemCategories(_userContext.Id).ToList();
        }

        public string GetCurrentOrderMessage()
        {
            if (_isCurrentOrderOnHold)
                return "No available waiters now. You need to wait";

            switch (CurrentOrderState)
            {
                case OrderState.Placed:
                    return "Order was placed";
                case OrderState.Accepted:
                    return "Order was accepted";
                case OrderState.NotRealized:
                    return "Order was not realized";
                case OrderState.Realized:
                    return "Order was realized";
                default:
                    throw new ArgumentException("This error shouldn't be catched");
            }
        }

        public void SetTargetMessage(IOrderViewModel orderViewModel)
        {
            _orderNotyficator.SetTarget(orderViewModel);
        }

        public void AddClient(string firstName, string lastName, string login, string password)
        {
            _clientDataAccess.AddClient(firstName, lastName, login, HashClass.CreateFirstHash(password, login));
        }

        public void AddOrder()
        {
            _currentOrder = _clientDataAccess.AddOrder(_userContext.Id, _tableId, MenuItems.Select(item => new TupleOfintint {m_Item1 = item.MenuItem.Id, m_Item2 = item.Quantity}).ToArray());
            CurrentOrderState = OrderState.Placed;
        }

        public IList<Table> GetTables()
        {
            return _clientDataAccess.GetTables(_userContext.Id).ToList();
        }

        public void SetTableId(int tableId)
        {
            _tableId = tableId;
        }

        public void SetOrderState(OrderState state)
        {
            CurrentOrderState = state;
        }

        public void Login(string login, string password)
        {
            _userContext = _clientDataAccess.LogIn(login, HashClass.CreateFirstHash(password, login));
        }

        public void Pay()
        {
            _clientDataAccess.PayForOrder(_userContext.Id, _currentOrder.Id);
        }

        public bool IsLogged()
        {
            return _userContext != null;
        }

        private MenuItemQuantity FindThisTypeOfOrder(MenuItem addingMenuItem)
        {
            var thisTypeOfOrder = MenuItems.FirstOrDefault(a => a.MenuItem.Name == addingMenuItem.Name);

            return thisTypeOfOrder;
        }

        public void SetCurrentOrderOnHold()
        {
            _isCurrentOrderOnHold = true;
        }

        public void LogOut()
        {
            if(IsLogged())
                _clientDataAccess.LogOut(_userContext.Id);
        }


        public void RemoveFromCurrentOrder(MenuItemQuantity removingItem, int count)
        {
            if(removingItem.Quantity < count)
                throw new ArgumentException("There is not as mach thist type menu item");

            if(removingItem.Quantity == 1)
                MenuItems.Remove(removingItem);

            removingItem.Quantity -= count;
        }
    }
}