using OrderClient.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderClient.ClientDataAccessWCFService;
using System.Windows;

namespace OrderClient.Model
{
    class OrderDataModel: IOrderDataModel
    {
        private IClientDataAccess _clientDataAccess;
        private IOrderNotyficator _orderNotyficator;

        private UserContext _userContext;
        private int _tableId;

        public IList<MenuItemQuantity> MenuItems { get; set; }

        public OrderDataModel(IClientDataAccess clientDataAccess, IOrderNotyficator orderNotyficator)
        {
            _clientDataAccess = clientDataAccess;
            _orderNotyficator = orderNotyficator;

            MenuItems = new List<MenuItemQuantity>();
        }

        public void AddToCurrentOrder(MenuItem addingMenuItem)
        {
            var TypeOfAddingOrder = FindThisTypeOfOrder(addingMenuItem);

            if(TypeOfAddingOrder == null)
            {
                MenuItems.Add(new MenuItemQuantity() { MenuItem = addingMenuItem, Quantity = 1 });
            }
            else
            {
                TypeOfAddingOrder.Quantity++;
            }
        }

        private MenuItemQuantity FindThisTypeOfOrder(MenuItem addingMenuItem)
        {
            var ThisTypeOfOrder = MenuItems.FirstOrDefault(a => a.MenuItem.Name == addingMenuItem.Name);

            return ThisTypeOfOrder;
        }

        public bool IsEmpty()
        {
            return MenuItems.Count == 0;
        }


        public void StartNewOrder()
        {
            MenuItems = new List<MenuItemQuantity>();
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
            return "Waiter has taken an order";
        }

        public void SetTargetMessage(IOrderViewModel orderViewModel)
        {
            _orderNotyficator.SetTarget(orderViewModel);
        }


        public void AddClient(string firstName, string lastName, string login, string password)
        {
            _clientDataAccess.AddClient(firstName, lastName, login, ClassLib.DataStructures.HashClass.CreateFirstHash(password, login));
            _userContext = _clientDataAccess.LogIn(firstName, ClassLib.DataStructures.HashClass.CreateFirstHash(password, login));

            if (_userContext != null)
                MessageBox.Show("Success");
        }




        public void AddOrder()
        {
            var m = new List<TupleOfintint>();
            foreach(var item in MenuItems)
            {
                m.Add(new TupleOfintint() { m_Item1 = item.MenuItem.Id, m_Item2 = item.Quantity });
            }
            _clientDataAccess.AddOrder(_userContext.Id, _clientDataAccess.GetTables(_userContext.Id)[0].Id, m.ToArray());
        }


        public IList<Table> GetTables()
        {
            return _clientDataAccess.GetTables(_userContext.Id).ToList();
        }


        public void SetTableId(int tableId)
        {
            _tableId = tableId;
        }
    }
}
