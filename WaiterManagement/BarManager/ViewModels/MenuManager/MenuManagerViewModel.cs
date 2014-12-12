using BarManager.Abstract;
using Caliburn.Micro;
using ClassLib.DbDataStructures;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BarManager.ViewModels
{
    /// <summary>
    /// Klasa odpowiedzialna za przetwarzanie danych w menu
    /// </summary>
    public class MenuManagerViewModel : Conductor<object>, IMenuManagerViewModel
    {
        private IBarDataModel DataModel;

        public MenuItemCategory SelectedCategory { get; set; }
        public MenuItem SelectedMenuItem { get; set; }

        private static MenuItemCategory AllItemsFlag;

        public IList<MenuItem> AllMenuItems { get; set; }

        private BindableCollection<MenuItem> _menuItems;
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

        public MenuManagerViewModel(IBarDataModel dataModel)
        {
            DataModel = dataModel;

            AllMenuItems = new ObservableCollection<MenuItem>();
            MenuItems = new BindableCollection<MenuItem>();

            Categories = new ObservableCollection<MenuItemCategory>();
            AvailableCategories = new ObservableCollection<MenuItemCategory>();

            InitializeData();
        }

        private void InitializeData()
        {
            Categories = new ObservableCollection<MenuItemCategory>(DataModel.GetAllCategories());
            foreach( var category in Categories)
            {
                AvailableCategories.Add(category);
            }

            AllItemsFlag = new MenuItemCategory() { Name = "All" };
            Categories.Add(AllItemsFlag);

            SelectedCategory = AllItemsFlag;

            AllMenuItems = new ObservableCollection<MenuItem>(DataModel.GetAllMenuItems());

            foreach( var menuItem in AllMenuItems)
            {
                MenuItems.Add(menuItem);
            }
        }

        private MenuItemCategory FindCategory(int id)
        {
            foreach(var cat in Categories)
            {
                if (cat.Id == id)
                    return cat;
            }

            return null;
        }

        public bool DeleteSelectedItem(out string error)
        {
            if (SelectedMenuItem == null)
            {
                error = "No Item Is Selected";
                return false;
            }
            else
            {
                if (DataModel.DeleteItem(SelectedMenuItem.Id))
                {
                    AllMenuItems.Remove(SelectedMenuItem);
                    MenuItems.Remove(SelectedMenuItem);
                    error = "";
                    return true;
                }
                else
                {
                    error = "Failed";
                    return false;
                }
            }
        }



        public void ShowCurrentCategory(MenuItemCategory category)
        {
            MenuItems.Clear();

            if(category == AllItemsFlag)
            {
                foreach(var menuItem in AllMenuItems)
                {
                    MenuItems.Add(menuItem);
                }
            }

            

            foreach(var menuItem in AllMenuItems)
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


        public void AddCategory(MenuItemCategory addingCategory)
        {
            Categories.Add(addingCategory);
            AvailableCategories.Add(addingCategory);
        }
    }
}
