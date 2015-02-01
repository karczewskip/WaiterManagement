using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using OrderClient.Abstract;
using OrderClient.ClientDataAccessWCFService;

namespace OrderClient.ViewModels
{
    internal class AddItemViewModel : PropertyChangedBase, IDialogAddingItem
    {
        private readonly IOrderDataModel _orderDataModel;
        private MenuItemCategory _allCategoryItem;
        private IOrderViewModel _orderWindow;
        private MenuItemCategory _selectedCategory;

        public AddItemViewModel(IOrderDataModel orderDataModel)
        {
            _orderDataModel = orderDataModel;

            InitializeMenu();
        }

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

        public void SetOrderWindowReference(IOrderViewModel orderViewModel)
        {
            _orderWindow = orderViewModel;
        }

        private void InitializeMenu()
        {
            MenuItems = new List<MenuItem>();

            _allCategoryItem = new MenuItemCategory {Name = "All", Description = "All"};
            Categories = new List<MenuItemCategory> {_allCategoryItem};

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

            if (_selectedCategory.Name == "All")
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