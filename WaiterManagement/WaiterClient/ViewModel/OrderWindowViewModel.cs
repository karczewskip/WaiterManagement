using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaiterClient.Abstract;
using DataAccess;
using ClassLib.DbDataStructures;
using System.Windows;
using System.Collections.ObjectModel;

namespace WaiterClient.ViewModel
{
    public class OrderWindowViewModel : IOrderWindowViewModel
    {
        private IWaiterClientModel WaiterClientModel;

        private int WaiterId;

        public IList<Table> ListOfTables { get; set; }
        public IList<MenuItem> ListOfMenuItems { get; set; }
        public IList<MenuItemCategory> ListOfCategories { get; set; }
        public IList<Order> ListOfOrders { get; set; }

        public OrderWindowViewModel(IWaiterClientModel waiterClientModel)
        {
            WaiterClientModel = waiterClientModel;

            Initialize();
        }

        private void Initialize()
        {
            ListOfTables = WaiterClientModel.GetTables();
            ListOfMenuItems = WaiterClientModel.GetMenuItems();
            ListOfCategories = WaiterClientModel.GetCategories();

            ListOfOrders = new ObservableCollection<Order>();
        }

        public void LogOut()
        {
            WaiterClientModel.LogOut(WaiterId);
        }


        public void InitializeUser(int id)
        {
            WaiterId = id;

            ListOfOrders.Clear();

            foreach (var o in WaiterClientModel.GetActiveOrders(id))
                ListOfOrders.Add(o);
            
        }


        public bool AddNewOrder(Table SelectedTable, IList<MenuItemQuantity> ListOfItems, out string error)
        {
            var addingOrder = WaiterClientModel.AddNewOrder(WaiterId, SelectedTable.Id, ListOfItems);

            if(addingOrder == null)
            {
                error = "Failed";
                return false;
            }
            else
            {
                ListOfOrders.Add(addingOrder);
                error = "";
                return true;
            }

        }
    }
}
