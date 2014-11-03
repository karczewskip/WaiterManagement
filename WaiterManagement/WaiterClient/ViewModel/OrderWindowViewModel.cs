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
        private IArchivedOrdersViewModel ArchivedOrdersViewModel;

        private int WaiterId;

        public IList<Table> ListOfTables { get; set; }
        public IList<MenuItem> ListOfMenuItems { get; set; }
        public IList<MenuItemCategory> ListOfCategories { get; set; }
        public IList<Order> ListOfOrders { get; set; }

        public Order SelectedOrder { get; set; }

        public OrderWindowViewModel(IWaiterClientModel waiterClientModel , IArchivedOrdersViewModel archivedOrdersViewModel)
        {
            WaiterClientModel = waiterClientModel;
            ArchivedOrdersViewModel = archivedOrdersViewModel;

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


        public bool InitializeUser(int id, out string error)
        {
            WaiterId = id;

            ArchivedOrdersViewModel.InitializeUser(id);

            ListOfOrders.Clear();

            IList<Order> ActiveOrdersList;

            try
            {
                ActiveOrdersList = WaiterClientModel.GetActiveOrders(id);
            }
            catch
            {
                error = "Failed with getting orders!";
                return false;
            }

            foreach (var o in ActiveOrdersList)
                ListOfOrders.Add(o);

            error = "";
            return true;
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


        public bool CancelOrder(out string error)
        {
            if (SelectedOrder == null)
            {
                error = "No Order Is Sellected";
                return false;
            }
            else
            {
                if (WaiterClientModel.CancelOrder(WaiterId, SelectedOrder.Id))
                {
                    ArchivedOrdersViewModel.AddArchivedOrder(SelectedOrder);
                    ListOfOrders.Remove(SelectedOrder);
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


        public bool RelizeOrder(out string error)
        {
            if (SelectedOrder == null)
            {
                error = "No Order Is Sellected";
                return false;
            }
            else
            {
                if (WaiterClientModel.RelizeOrder(WaiterId, SelectedOrder.Id))
                {
                    ArchivedOrdersViewModel.AddArchivedOrder(SelectedOrder);
                    ListOfOrders.Remove(SelectedOrder);
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
    }
}
