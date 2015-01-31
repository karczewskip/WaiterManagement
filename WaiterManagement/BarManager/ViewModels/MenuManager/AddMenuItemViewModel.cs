using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarManager.Abstract;
using BarManager.ManagerDataAccessWCFService;
using System.Windows;
using System.ComponentModel;
using BarManager.Abstract.Model;
using BarManager.Abstract.ViewModel;
using BarManager.Messaging;

namespace BarManager.ViewModels
{
    /// <summary>
    /// Klasa odpowiedzialna za dodawanie pozycji w menu
    /// </summary>
    public class AddMenuItemViewModel : IAddMenuItemViewModel, INotifyPropertyChanged
    {
        private IBarDataModel DataModel;
        private IMenuManagerViewModel MenuManagerViewModel;

        public string MenuItemName { get; set; }
        public string Price { get; set; }
        public MenuItemCategory SelectedCategory { get; set; }
        public IList<MenuItemCategory> Categories { get { return MenuManagerViewModel.AvailableCategories; } }
        public string MenuItemDescription { get; set; }


        public AddMenuItemViewModel(IBarDataModel dataModel, IMenuManagerViewModel menuManagerViewModel)
        {
            DataModel = dataModel;
            MenuManagerViewModel = menuManagerViewModel;

            SelectedCategory = null;
        }

        public void AddItem()
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

            if (MenuManagerViewModel.AllMenuItems.Any(cat => cat.Name.Equals(MenuItemName)))
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


            var AddingMenuItem = DataModel.AddMenuItem(MenuItemName, SelectedCategory, price, MenuItemDescription);

            if (AddingMenuItem != null)
            {
                MenuManagerViewModel.AddNewMenuItem(AddingMenuItem);
                MenuManagerViewModel.CloseDialogs();
                return;
            }

            Message.Show("Falied");

            return;
        }


        public void Clear()
        {
            MenuItemName = "";
            MenuItemDescription = "";
            Price = "";

            SelectedCategory = null;

            if (null != this.PropertyChanged)
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
