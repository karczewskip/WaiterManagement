using Caliburn.Micro;
using ClassLib.DataStructures;
using ClassLib.DbDataStructures;
using OrderClient.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OrderClient.ViewModels
{
    class AddItemViewModel : PropertyChangedBase, IDialogOrder
    {
        private IOrderViewModel _orderWindow;
        private IOrderDataModel _orderDataModel;

        private MenuItemCategory _allCategoryItem;

        private MenuItemCategory _selectedCategory;
        public MenuItemCategory SelectedCategory 
        {
            get { return _selectedCategory; }
            set
            {
                _selectedCategory = value;
                RefreshMenuItemList();
            }
        }

        public MenuItem SelectedMenuItem { get; set; }

        public IList<MenuItem> MenuItems { get; set; }
        public IList<MenuItemCategory> Categories { get; set; }

        public AddItemViewModel(IOrderViewModel orderWindow, IOrderDataModel orderDataModel)
        {
            _orderWindow = orderWindow;
            _orderDataModel = orderDataModel;


            InitializeMenu();
        }

        private void InitializeMenu()
        {
            MenuItems = new List<MenuItem>();

            //_allCategoryItem = new MenuItemCategory() { Name = "All", Description = "All" };
            Categories = new List<MenuItemCategory>() { _allCategoryItem };

            foreach (var category in _orderDataModel.GetAllCategories())
                Categories.Add(category);

            SelectedCategory = _allCategoryItem;
        }
        
        public void AddNewItem(MenuItem addingMenuItem)
        {
            _orderDataModel.AddToCurrentOrder(addingMenuItem);
        }

        public void Exit()
        {
            _orderWindow.CloseAddItemDialog();
        }

        private void RefreshMenuItemList()
        {
            MenuItems.Clear();

            if(_selectedCategory.Name == "All")
            {
                MenuItems = _orderDataModel.GetAllItems();
            }
            else
            {
                MenuItems = _orderDataModel.GetAllItems().Where(m => m.Category.Name == _selectedCategory.Name).ToList();
            }

            NotifyOfPropertyChange(() => MenuItems);
        }
    }
}
