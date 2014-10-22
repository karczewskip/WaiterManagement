using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarManager.Abstract;
using ClassLib.DbDataStructures;
using System.Windows;

namespace BarManager.ViewModel
{
    class EditMenuItemViewModel: IEditMenuItemViewModel
    {
        private IMenuManagerViewModel MenuManagerViewModel;
        private IBarDataModel DataModel;

        private MenuItem MenuItem;

        public string MenuItemName { get; set; }
        public string PriceString { get; set; }
        public MenuItemCategory SelectedCategory { get; set; }
        public string MenuItemDescription { get; set; }

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
            SelectedCategory = menuItem.Category;
            MenuItemDescription = menuItem.Description;
        }


        public bool EditMenuItem()
        {
            if (string.IsNullOrEmpty(MenuItemName) || string.IsNullOrEmpty(PriceString) || string.IsNullOrEmpty(MenuItemDescription))
            {
                MessageBox.Show("Some Fields are empty");
                return false;
            }

            if (SelectedCategory == null)
            {
                MessageBox.Show("No Category was selected");
                return false;
            }

            if (MenuManagerViewModel.ListOfMenuItems.Any(cat => (cat.Name.Equals(MenuItemName) && cat.Id != MenuItem.Id)))
            {
                MessageBox.Show("There is menu item named: " + MenuItemName);
                return false;
            }

            double Price;

            if (!double.TryParse(PriceString, out Price))
            {
                MessageBox.Show("Price is wrong");
                return false;
            }

            var result = DataModel.EditMenuItem(MenuItem, MenuItemName, Price, SelectedCategory, MenuItemDescription);
            

            return result;
        }
    }
}
