using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarManager.Abstract;
using ClassLib.DbDataStructures;
using System.Windows;
using System.ComponentModel;
using BarManager.Messaging;

namespace BarManager.ViewModels
{
    /// <summary>
    /// Klasa odpowiedzialna za edytowanie pozycji w menu
    /// </summary>
    class EditMenuItemViewModel : IEditMenuItemViewModel, INotifyPropertyChanged
    {
        private IMenuManagerViewModel MenuManagerViewModel;
        private IBarDataModel DataModel;

        private MenuItem MenuItem;

        private string menuItemName;
        public string MenuItemName 
        {
            get { return menuItemName; }
            set
            {
                menuItemName = value;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("MenuItemName"));
                }
            }
        }

        private string priceString;
        public string Price 
        {
            get { return priceString; }
            set 
            {
                priceString = value;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("PriceString"));
                }
            }
        }

        private MenuItemCategory selectedCategory;
        public MenuItemCategory SelectedCategory 
        {
            get { return selectedCategory; }
            set
            {
                selectedCategory = value;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedCategory"));
                }
            }
        }

        private string menuItemDiscription;
        public string MenuItemDescription 
        {
            get { return menuItemDiscription; }
            set 
            {
                menuItemDiscription = value;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("MenuItemDescription"));
                }
            }
        }

        public IList<MenuItemCategory> Categories { get { return MenuManagerViewModel.AvailableCategories; } }

        public EditMenuItemViewModel(IBarDataModel dateModel, IMenuManagerViewModel menuManagerViewModel)
        {
            DataModel = dateModel;
            MenuManagerViewModel = menuManagerViewModel;
        }

        public void RefreshItem(MenuItem menuItem)
        {
            MenuItem = menuItem;

            MenuItemName = menuItem.Name;
            Price = menuItem.Price.Amount.ToString();
            SelectedCategory = Categories.First( c => c.Id == menuItem.Category.Id );
            MenuItemDescription = menuItem.Description;
        }


        public void EditMenuItem()
        {
            if (string.IsNullOrEmpty(MenuItemName) || string.IsNullOrEmpty(Price) || string.IsNullOrEmpty(MenuItemDescription))
            {
                Message.Show("Some Fields are empty");
                return;
            }

            if (SelectedCategory == null)
            {
                Message.Show("No Category was selected");
                return;
            }

            if (MenuManagerViewModel.AllMenuItems.Any(cat => (cat.Name.Equals(MenuItemName) && cat.Id != MenuItem.Id)))
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

            var result = DataModel.EditMenuItem(MenuItem, MenuItemName, price, SelectedCategory, MenuItemDescription);

            if (result)
            {
                MenuManagerViewModel.MenuItems.Refresh();
                MenuManagerViewModel.CloseDialogs();
            }
            else
            {
                Message.Show("Failed");
            }

            return ;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion


    }
}
