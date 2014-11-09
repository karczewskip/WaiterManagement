using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarManager.Abstract;
using ClassLib.DbDataStructures;
using System.Windows;
using System.ComponentModel;

namespace BarManager.ViewModel
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
        public string PriceString 
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

        public IList<MenuItemCategory> ListOfCategories { get { return MenuManagerViewModel.ShowingCategories; } }

        public EditMenuItemViewModel(IBarDataModel dateModel, IMenuManagerViewModel menuManagerViewModel)
        {
            DataModel = dateModel;
            MenuManagerViewModel = menuManagerViewModel;
        }

        public void RefreshItem(MenuItem menuItem)
        {
            MenuItem = menuItem;

            MenuItemName = menuItem.Name;
            PriceString = menuItem.Price.Amount.ToString();
            SelectedCategory = ListOfCategories.First( c => c.Id == menuItem.Category.Id );
            MenuItemDescription = menuItem.Description;
        }


        public bool EditMenuItem(out string error)
        {
            if (string.IsNullOrEmpty(MenuItemName) || string.IsNullOrEmpty(PriceString) || string.IsNullOrEmpty(MenuItemDescription))
            {
                error = "Some Fields are empty";
                return false;
            }

            if (SelectedCategory == null)
            {
                error = "No Category was selected";
                return false;
            }

            if (MenuManagerViewModel.ListOfMenuItems.Any(cat => (cat.Name.Equals(MenuItemName) && cat.Id != MenuItem.Id)))
            {
                error = "There is menu item named: " + MenuItemName;
                return false;
            }

            double Price;

            if (!double.TryParse(PriceString, out Price))
            {
                error = "Price is wrong";
                return false;
            }

            var result = DataModel.EditMenuItem(MenuItem, MenuItemName, Price, SelectedCategory, MenuItemDescription);

            if (result)
                error = "";
            else
                error = "Failed";

            return result;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion


    }
}
