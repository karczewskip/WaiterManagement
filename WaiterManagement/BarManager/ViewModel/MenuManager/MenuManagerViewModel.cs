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

        public MenuItem SelectedMenuItem { get; set; }

        public IList<MenuItem> ListOfMenuItems { get; set; }
        public IList<MenuItemCategory> ListOfCategories { get; set; }

        public MenuManagerViewModel(IBarDataModel dataModel)
        {
            DataModel = dataModel;

            ListOfMenuItems = new ObservableCollection<MenuItem>();
            ListOfCategories = new ObservableCollection<MenuItemCategory>();

            InitializeData();
        }

        private void InitializeData()
        {
            ListOfCategories = new ObservableCollection<MenuItemCategory>(DataModel.GetAllCategories());
            ListOfMenuItems = new ObservableCollection<MenuItem>(DataModel.GetAllMenuItems());

            //for (int i = 0; i < ListOfMenuItems.Count; i++)
            //{
            //    ListOfMenuItems[i].Category = FindCategor(ListOfMenuItems[i].Category.Id);
            //}
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



        public void DeleteSelectedItem()
        {
            if (SelectedMenuItem == null)
                MessageBox.Show("No Item Is Sellected");
            else 
            {
                if (DataModel.DeleteItem(SelectedMenuItem.Id))
                    ListOfMenuItems.Remove(SelectedMenuItem);
                else
                    MessageBox.Show("Failed");
            }
        }

    }
}
