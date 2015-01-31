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
    ///     Klasa odpowiedzialna za dodawanie pozycji w menu
    /// </summary>
    public class AddMenuItemViewModel : IAddMenuItemViewModel, INotifyPropertyChanged
    {
        private readonly IMenuDataModel _menuDataModel;
        private readonly IMenuManagerViewModel _menuManagerViewModel;

        public AddMenuItemViewModel(IMenuDataModel dataModel, IMenuManagerViewModel menuManagerViewModel)
        {
            _menuDataModel = dataModel;
            _menuManagerViewModel = menuManagerViewModel;

            SelectedCategory = null;
        }

        public string MenuItemName { get; set; }
        public string Price { get; set; }
        public MenuItemCategory SelectedCategory { get; set; }

        public IList<MenuItemCategory> Categories
        {
            get { return _menuManagerViewModel.AvailableCategories; }
        }

        public string MenuItemDescription { get; set; }

        public void AddItem()
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

            if (_menuManagerViewModel.AllMenuItems.Any(cat => cat.Name.Equals(MenuItemName)))
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


            var addingMenuItem = _menuDataModel.AddMenuItem(MenuItemName, SelectedCategory, price, MenuItemDescription);

            if (addingMenuItem != null)
            {
                _menuManagerViewModel.AddNewMenuItem(addingMenuItem);
                _menuManagerViewModel.CloseDialogs();
                return;
            }

            Message.Show("Falied");
        }

        public void Clear()
        {
            MenuItemName = "";
            MenuItemDescription = "";
            Price = "";

            SelectedCategory = null;

            if (null != PropertyChanged)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("MenuItemName"));
                PropertyChanged(this, new PropertyChangedEventArgs("MenuItemDescription"));
                PropertyChanged(this, new PropertyChangedEventArgs("PriceString"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedCategory"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}