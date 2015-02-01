using System.Linq;
using System.Net.Mime;
using Caliburn.Micro;
using ClassLib;
using OrderClient.Abstract;
using OrderClient.ClientDataAccessWCFService;

namespace OrderClient.ViewModels
{
    internal class CurrentOrderViewModel : PropertyChangedBase, IDialogOrder, ICurrentOrder
    {
        private readonly IOrderDataModel _orderDataModel;
        private readonly IOrderViewModel _orderWindow;
        private BindableCollection<MenuItemQuantity> _menuItems;

        public CurrentOrderViewModel(IOrderViewModel orderWindow, IOrderDataModel orderDataModel)
        {
            _orderWindow = orderWindow;
            _orderDataModel = orderDataModel;

            _menuItems = new BindableCollection<MenuItemQuantity>();
        }

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
            get { return CalculateSalary() + " " + ApplicationResources.DefaultCurrency + " to pay"; }
        }

        public void RefreshOrder()
        {
            MenuItems = new BindableCollection<MenuItemQuantity>(_orderDataModel.MenuItems);
            NotifyOfPropertyChange(() => Salary);
            _orderWindow.CheckIfIsPosibleToAddOrder();
        }

        private float CalculateSalary()
        {
            return MenuItems.Sum(m => m.Quantity*m.MenuItem.Price.Amount);
        }

        public void RemoveItem(MenuItemQuantity removingItem)
        {
            _orderDataModel.RemoveFromCurrentOrder(removingItem,1);
            RefreshOrder();
        }
    }
}