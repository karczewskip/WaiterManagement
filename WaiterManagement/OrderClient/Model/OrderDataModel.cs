using OrderClient.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderClient.ClientDataAccessWCFService;

namespace OrderClient.Model
{
    class OrderDataModel: IOrderDataModel
    {
        private IClientDataAccess _clientDataAccess;

        public Order CurrentOrder { get; set; }

        public OrderDataModel(IClientDataAccess clientDataAccess)
        {
            _clientDataAccess = clientDataAccess;

            CurrentOrder = new Order();
        }

        public void AddToCurrentOrder(MenuItem addingMenuItem)
        {
            var TypeOfAddingOrder = FindThisTypeOfOrder(addingMenuItem);

            if(TypeOfAddingOrder == null)
            {
                //CurrentOrder.MenuItems.Add(new MenuItemQuantity() { MenuItem = addingMenuItem, Quantity = 1 });
            }
            else
            {
                TypeOfAddingOrder.Quantity++;
            }
        }

        private MenuItemQuantity FindThisTypeOfOrder(MenuItem addingMenuItem)
        {
            var ThisTypeOfOrder = CurrentOrder.MenuItems.FirstOrDefault(a => a.MenuItem.Name == addingMenuItem.Name);

            return ThisTypeOfOrder;
        }

        public bool IsEmpty()
        {
            return true; //CurrentOrder.MenuItems.Count == 0;
        }


        public void StartNewOrder()
        {
            CurrentOrder = new Order();
        }


        public void RemoveFromCurrentOrder(MenuItemQuantity removingItem)
        {
            //CurrentOrder.MenuItems.Remove(removingItem);
        }


        public IList<MenuItem> GetAllItems()
        {
            return new List<MenuItem>() 
            { 
                //new MenuItem() { Name = "Schabowy", Category = new MenuItemCategory() { Name = "Dania", Description="Dania na ciepło" }, Description = "Świetne Danie" , Price = new Money() { Amount = 20, Currency = "PLN"}},
                //new MenuItem() { Name = "Wódka", Category = new MenuItemCategory() { Name = "Napoje", Description="Alkohole" }, Description = "Świetna Wódka", Price = new Money() { Amount = 30, Currency = "PLN"} } 
            };
        }


        public IList<MenuItemCategory> GetAllCategories()
        {
            return new List<MenuItemCategory>() 
            { 
                //new MenuItemCategory() { Name = "Dania", Description = "Dania na ciepło" }, new MenuItemCategory() { Name = "Napoje", Description = "Alkohole" } 
            };
        }


        public string GetCurrentOrderMessage()
        {
            return "Waiter has taken an order";
        }


        public void LogIn(string _userName, string password)
        {
            //_clientDataAccess.AddClient()
        }
    }
}
