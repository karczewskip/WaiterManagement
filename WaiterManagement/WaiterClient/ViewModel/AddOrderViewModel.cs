using ClassLib.DbDataStructures;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WaiterClient.Abstract;

namespace WaiterClient.ViewModel
{
    /// <summary>
    /// Klasa odpowiedzialna za dodawanie zamówienia
    /// </summary>
    public class AddOrderViewModel : IAddOrderViewModel, INotifyPropertyChanged
    {
        private IOrderWindowViewModel OrderWindowViewModel;
        private System.Windows.Controls.ListView ItemsListView;

        public Table SelectedTable { get; set; }
        public IList<Table> ListOfTables { get { return OrderWindowViewModel.ListOfTables; } }
        public IList<MenuItemQuantity> ListOfItems { get; set; }
        public MenuItemQuantity SelectedItem { get; set; }

        public AddOrderViewModel(IOrderWindowViewModel orderWindowViewModel)
        {
            OrderWindowViewModel = orderWindowViewModel;
            ListOfItems = new ObservableCollection<MenuItemQuantity>();
        }


        public void AddItem(MenuItem menuItem)
        {
            var item = ListOfItems.FirstOrDefault(c => c.MenuItem.Name == menuItem.Name);

            if (item != null)
            {
                item.Quantity++;
                ItemsListView.Items.Refresh();
            }
            else
                ListOfItems.Add(new MenuItemQuantity() { MenuItem = menuItem, Quantity = 1 });
        }


        public void AddObserverListView(System.Windows.Controls.ListView itemsListView)
        {
            ItemsListView = itemsListView;
        }


        public bool DeleteSelectedItem(out string error)
        {
            if (ListOfItems.Remove(SelectedItem))
            {
                error = "";
                return true;
            }
            else
            {
                error = "Failed";
                return false;
            }
        }


        public bool AddOrder(out string error)
        {
            if(SelectedTable == null)
            {
                error = "No Table was selected";
                return false;
            }

            if(ListOfItems.Count == 0)
            {
                error = "Nothing was ordered";
                return false;
            }

            return OrderWindowViewModel.AddNewOrder(SelectedTable, ListOfItems, out error);
        }

        public void Clear()
        {
            SelectedTable = null;
            SelectedItem = null;

            ListOfItems.Clear();

            if (null != this.PropertyChanged)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedTable"));

            }
        }


        public event PropertyChangedEventHandler PropertyChanged;


    }
}
