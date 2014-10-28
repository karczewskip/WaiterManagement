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
    public class AddMenuItemViewModel : IAddMenuItemViewModel
    {
        private IBarDataModel DataModel;
        private IMenuManagerViewModel MenuManagerViewModel;

        public string MenuItemName { get; set; }
        public string PriceString { get; set; }
        public MenuItemCategory SelectedCategory { get; set; }
        public IList<MenuItemCategory> ListOfCategories { get { return MenuManagerViewModel.ListOfCategories; } }
        public string MenuItemDescription { get; set; }


        public AddMenuItemViewModel(IBarDataModel dataModel, IMenuManagerViewModel menuManagerViewModel)
        {
            DataModel = dataModel;
            MenuManagerViewModel = menuManagerViewModel;

            SelectedCategory = null;
        }

        public bool AddMenuItem(out string error)
        {
            if(string.IsNullOrEmpty(MenuItemName) || string.IsNullOrEmpty(PriceString) || string.IsNullOrEmpty(MenuItemDescription) )
            {
                error = "Some Fields are empty";
                return false;
            }

            if(SelectedCategory == null)
            {
                error = "No Category was selected";
                return false;
            }

            if (MenuManagerViewModel.ListOfMenuItems.Any(cat => cat.Name.Equals(MenuItemName)))
            {
                error = "There is menu item named: " + MenuItemName;
                return false;
            }

            double Price;

            if(!double.TryParse(PriceString, out Price))
            {
                error = "Price is wrong";
                return false;
            }

            
            var AddingMenuItem = DataModel.AddMenuItem(MenuItemName, SelectedCategory, Price, MenuItemDescription);

            if (AddingMenuItem != null)
            {
                MenuManagerViewModel.ListOfMenuItems.Add(AddingMenuItem);
                error = "";
                return true;
            }

            error = "Falied";

            return false;



        }
    }
}
