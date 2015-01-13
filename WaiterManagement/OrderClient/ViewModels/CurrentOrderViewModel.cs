using Caliburn.Micro;
using ClassLib.DataStructures;
using OrderClient.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderClient.ViewModels
{
    class CurrentOrderViewModel : PropertyChangedBase, IDialogOrder, ICurrentOrder
    {
        private IOrderViewModel _orderWindow;
        private IOrderDataModel _orderDataModel;

        private BindableCollection<MenuItemQuantity> _menuItems;
        public BindableCollection<MenuItemQuantity> MenuItems
        {
            get { return _menuItems; }
            set
            {
                _menuItems = value;
                NotifyOfPropertyChange(() => MenuItems);
            }
        }

        public string Salary
        {
            get { return CalculateSalary() + " PLN to pay"; }
        }

        private float CalculateSalary()
        {
            float sum = 0;

            foreach (var m in MenuItems)
                sum += m.Quantity * m.MenuItem.Price.Amount;

            return sum;
        }

        public CurrentOrderViewModel(IOrderViewModel orderWindow, IOrderDataModel orderDataModel)
        {
            _orderWindow = orderWindow;
            _orderDataModel = orderDataModel;

            _menuItems = new BindableCollection<MenuItemQuantity>();
        }

        public void RemoveItem(MenuItemQuantity removingItem)
        {
            _orderDataModel.RemoveFromCurrentOrder(removingItem);
            RefreshOrder();
        }

        public void RefreshOrder()
        {
            MenuItems = new BindableCollection<MenuItemQuantity>(_orderDataModel.CurrentOrder.MenuItems);
            NotifyOfPropertyChange(() => Salary);
            _orderWindow.CheckIfIsPosibleToAddOrder();
        }
    }
}
