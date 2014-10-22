using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarManager.Abstract;
using ClassLib.DbDataStructures;
using System.Collections.ObjectModel;
using DataAccess;

namespace BarManager.ViewModel
{
    public class MenuManagerViewModel : IMenuManagerViewModel
    {
        private IBarDataModel DataModel;

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

            //for (int i = 0; i < ListOfMenuItems.Count; i++ )
            //{
            //    //ListOfMenuItems[i].Category = FindCategor(ListOfMenuItems[i].Category)
            //}
        }
        
    }
}
