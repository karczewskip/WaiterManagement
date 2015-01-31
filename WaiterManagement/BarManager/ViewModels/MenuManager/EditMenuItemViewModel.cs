using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using BarManager.Abstract.Model;
using BarManager.Abstract.ViewModel;
using BarManager.ManagerDataAccessWCFService;
using BarManager.Messaging;

namespace BarManager.ViewModels
{
    /// <summary>
    ///     Klasa odpowiedzialna za edytowanie pozycji w menu
    /// </summary>
    internal class EditMenuItemViewModel : IEditMenuItemViewModel, INotifyPropertyChanged
    {
        private readonly IMenuDataModel _menuDataModel;
        private readonly IMenuManagerViewModel _menuManagerViewModel;
        private MenuItem _menuItem;
        private string _menuItemDiscription;
        private string _menuItemName;
        private string _priceString;
        private MenuItemCategory _selectedCategory;

        public EditMenuItemViewModel(IMenuDataModel menuDataModel, IMenuManagerViewModel menuManagerViewModel)
        {
            _menuDataModel = menuDataModel;
            _menuManagerViewModel = menuManagerViewModel;
        }

        public string MenuItemName
        {
            get { return _menuItemName; }
            set
            {
                _menuItemName = value;
                if (null != PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("MenuItemName"));
                }
            }
        }

        public string Price
        {
            get { return _priceString; }
            set
            {
                _priceString = value;
                if (null != PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("PriceString"));
                }
            }
        }

        public MenuItemCategory SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                _selectedCategory = value;
                if (null != PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedCategory"));
                }
            }
        }

        public string MenuItemDescription
        {
            get { return _menuItemDiscription; }
            set
            {
                _menuItemDiscription = value;
                if (null != PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("MenuItemDescription"));
                }
            }
        }

        public IList<MenuItemCategory> Categories
        {
            get { return _menuManagerViewModel.AvailableCategories; }
        }

        public void RefreshItem(MenuItem menuItem)
        {
            _menuItem = menuItem;

            MenuItemName = menuItem.Name;
            Price = menuItem.Price.Amount.ToString();
            SelectedCategory = Categories.First(c => c.Id == menuItem.Category.Id);
            MenuItemDescription = menuItem.Description;
        }

        public void EditMenuItem()
        {
            if (string.IsNullOrEmpty(MenuItemName) || string.IsNullOrEmpty(Price) ||
                string.IsNullOrEmpty(MenuItemDescription))
            {
                Message.Show("Some Fields are empty");
                return;
            }

            if (SelectedCategory == null)
            {
                Message.Show("No Category was selected");
                return;
            }

            if (_menuManagerViewModel.AllMenuItems.Any(cat => (cat.Name.Equals(MenuItemName) && cat.Id != _menuItem.Id)))
            {
                Message.Show("There is menu item named: " + MenuItemName);
                return;
            }

            double price;

            if (!double.TryParse(Price, out price))
            {
                Message.Show("Price is wrong");
                return;
            }

            var result = _menuDataModel.EditMenuItem(_menuItem, MenuItemName, price, SelectedCategory, MenuItemDescription);

            if (result)
            {
                _menuManagerViewModel.MenuItems.Refresh();
                _menuManagerViewModel.CloseDialogs();
            }
            else
            {
                Message.Show("Failed");
            }
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}