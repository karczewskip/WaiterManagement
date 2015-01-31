using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BarManager.Abstract.Model;
using BarManager.Abstract.ViewModel;
using BarManager.ManagerDataAccessWCFService;
using Caliburn.Micro;
using Message = BarManager.Messaging.Message;

namespace BarManager.ViewModels
{
    /// <summary>
    ///     Klasa odpowiedzialna za przetwarzanie danych w menu
    /// </summary>
    public class MenuManagerViewModel : Conductor<object>, IMenuManagerViewModel
    {
        private static MenuItemCategory AllItemsFlag;
        private readonly IMenuDataModel _menuDataModel;
        private IAddCategoryViewModel _addCategoryViewModel;
        private IAddMenuItemViewModel _addMenuItemViewModel;
        private IEditMenuItemViewModel _editMenuItemViewModel;
        private BindableCollection<MenuItem> _menuItems;

        public MenuManagerViewModel(IMenuDataModel dataModel)
        {
            _menuDataModel = dataModel;

            AllMenuItems = new ObservableCollection<MenuItem>();
            MenuItems = new BindableCollection<MenuItem>();

            Categories = new ObservableCollection<MenuItemCategory>();
            AvailableCategories = new ObservableCollection<MenuItemCategory>();
        }

        public MenuItemCategory SelectedCategory { get; set; }
        public MenuItem SelectedMenuItem { get; set; }
        public IList<MenuItem> AllMenuItems { get; set; }

        public BindableCollection<MenuItem> MenuItems
        {
            get { return _menuItems; }
            set
            {
                _menuItems = value;
                NotifyOfPropertyChange(() => MenuItems);
            }
        }

        public IList<MenuItemCategory> Categories { get; set; }
        public IList<MenuItemCategory> AvailableCategories { get; set; }

        public void DeleteItem()
        {
            if (SelectedMenuItem == null)
            {
                Message.Show("No Item Is Selected");
            }
            else
            {
                if (_menuDataModel.DeleteItem(SelectedMenuItem.Id))
                {
                    AllMenuItems.Remove(SelectedMenuItem);
                    MenuItems.Remove(SelectedMenuItem);
                }
                else
                {
                    Message.Show("Failed");
                }
            }
        }

        public void ShowCurrentCategory(MenuItemCategory category)
        {
            MenuItems.Clear();

            if (category == AllItemsFlag)
            {
                foreach (var menuItem in AllMenuItems)
                {
                    MenuItems.Add(menuItem);
                }
            }

            foreach (var menuItem in AllMenuItems)
            {
                if (menuItem.Category.Id == category.Id)
                    MenuItems.Add(menuItem);
            }
        }

        public void AddNewMenuItem(MenuItem addingMenuItem)
        {
            AllMenuItems.Add(addingMenuItem);

            if (SelectedCategory == AllItemsFlag || SelectedCategory == addingMenuItem.Category)
                MenuItems.Add(addingMenuItem);
        }

        public void AddCategoryToViewModel(MenuItemCategory addingCategory)
        {
            Categories.Add(addingCategory);
            AvailableCategories.Add(addingCategory);
        }

        public void CloseDialogs()
        {
            CloseAddMenuItemDialog();
            CloseEditMenuItemDialog();
            CloseAddCategoryDialog();
        }

        public void RefreshData()
        {
            InitializeData();
        }

        private void InitializeData()
        {
            Categories = new ObservableCollection<MenuItemCategory>(_menuDataModel.GetAllCategories());
            foreach (var category in Categories)
            {
                AvailableCategories.Add(category);
            }

            AllItemsFlag = new MenuItemCategory {Name = "All", Description = "All"};
            Categories.Add(AllItemsFlag);

            SelectedCategory = AllItemsFlag;

            AllMenuItems = new ObservableCollection<MenuItem>(_menuDataModel.GetAllMenuItems());

            foreach (var menuItem in AllMenuItems)
            {
                MenuItems.Add(menuItem);
            }
        }

        private MenuItemCategory FindCategory(int id)
        {
            return Categories.FirstOrDefault(cat => cat.Id == id);
        }

        public void ChangeSelectedCategory()
        {
            ShowCurrentCategory(SelectedCategory);
        }

        public void AddItem()
        {
            _addMenuItemViewModel = IoC.Get<IAddMenuItemViewModel>();
            _addMenuItemViewModel.Clear();
            ActivateItem(_addMenuItemViewModel);
        }

        public void AddCategory()
        {
            _addCategoryViewModel = IoC.Get<IAddCategoryViewModel>();
            _addCategoryViewModel.Clear();
            ActivateItem(_addCategoryViewModel);
        }

        public void EditMenuItem()
        {
            if (SelectedMenuItem == null)
            {
                return;
            }

            _editMenuItemViewModel = IoC.Get<IEditMenuItemViewModel>();
            _editMenuItemViewModel.RefreshItem(SelectedMenuItem);
            ActivateItem(_editMenuItemViewModel);
        }

        private void CloseAddCategoryDialog()
        {
            DeactivateItem(_addCategoryViewModel, true);
        }

        private void CloseEditMenuItemDialog()
        {
            DeactivateItem(_editMenuItemViewModel, true);
        }

        private void CloseAddMenuItemDialog()
        {
            DeactivateItem(_addMenuItemViewModel, true);
        }
    }
}