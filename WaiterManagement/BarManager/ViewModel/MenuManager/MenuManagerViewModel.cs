using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarManager.Abstract;
using ClassLib.DbDataStructures;
using System.Collections.ObjectModel;
using DataAccess;
using System.Windows;
using System.ComponentModel;

namespace BarManager.ViewModel
{
    public class MenuManagerViewModel : IMenuManagerViewModel
    {
        private IBarDataModel DataModel;

        public MenuItemCategory SelectedCategory { get; set; }
        public MenuItem SelectedMenuItem { get; set; }

        private static MenuItemCategory AllItemsFlag;

        public IList<MenuItem> ListOfMenuItems { get; set; }
        public IList<MenuItem> ShowingMenuItems { get; set; }
        public IList<MenuItemCategory> ListOfCategories { get; set; }
        public IList<MenuItemCategory> ShowingCategories { get; set; }

        public MenuManagerViewModel(IBarDataModel dataModel)
        {
            DataModel = dataModel;

            ListOfMenuItems = new ObservableCollection<MenuItem>();
            ShowingMenuItems = new ObservableCollection<MenuItem>();
            ListOfCategories = new ObservableCollection<MenuItemCategory>();
            ShowingCategories = new ObservableCollection<MenuItemCategory>();

            InitializeData();
        }

        private void InitializeData()
        {
            ListOfCategories = new ObservableCollection<MenuItemCategory>(DataModel.GetAllCategories());
            foreach( var category in ListOfCategories)
            {
                ShowingCategories.Add(category);
            }

            AllItemsFlag = new MenuItemCategory() { Name = "All" };
            ListOfCategories.Add(AllItemsFlag);

            SelectedCategory = AllItemsFlag;

            ListOfMenuItems = new ObservableCollection<MenuItem>(DataModel.GetAllMenuItems());

            foreach( var menuItem in ListOfMenuItems)
            {
                ShowingMenuItems.Add(menuItem);
            }
        }

        private MenuItemCategory FindCategory(int id)
        {
            foreach(var cat in ListOfCategories)
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
                    ListOfMenuItems.Remove(SelectedMenuItem);
                    ShowingMenuItems.Remove(SelectedMenuItem);
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
            ShowingMenuItems.Clear();

            if(category == AllItemsFlag)
            {
                foreach(var menuItem in ListOfMenuItems)
                {
                    ShowingMenuItems.Add(menuItem);
                }
            }

            

            foreach(var menuItem in ListOfMenuItems)
            {
                if (menuItem.Category.Id == category.Id)
                    ShowingMenuItems.Add(menuItem);
            }
        }


        public void AddNewMenuItem(MenuItem addingMenuItem)
        {
            ListOfMenuItems.Add(addingMenuItem);

            if (SelectedCategory == AllItemsFlag || SelectedCategory == addingMenuItem.Category)
                ShowingMenuItems.Add(addingMenuItem);
        }


        public void AddCategory(MenuItemCategory addingCategory)
        {
            ListOfCategories.Add(addingCategory);
            ShowingCategories.Add(addingCategory);
        }
    }
}
