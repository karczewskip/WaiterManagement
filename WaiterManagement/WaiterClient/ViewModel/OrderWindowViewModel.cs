using System.Collections.Generic;
using WaiterClient.Abstract;
using System.Collections.ObjectModel;
using WaiterClient.WaiterDataAccessWCFService;

namespace WaiterClient.ViewModel
{
    /// <summary>
    /// Klasa odpowiedzialna za przetwarzanie danych zamówień
    /// </summary>
    public class OrderWindowViewModel : IOrderWindowViewModel
    {
        private IWaiterClientModel WaiterClientModel;
        private IArchivedOrdersViewModel ArchivedOrdersViewModel;

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
            WaiterClientModel.LogOut();
        }

        public bool InitializeUser(out string error)
        {
            ArchivedOrdersViewModel.InitializeUser();

            ListOfOrders.Clear();

            IList<Order> activeOrdersList;

            try
            {
                activeOrdersList = WaiterClientModel.GetActiveOrders();
            }
            catch
            {
                error = "Failed with getting orders!";
                return false;
            }

            foreach (var o in activeOrdersList)
                ListOfOrders.Add(o);

            error = "";
            return true;
        }

        public bool CancelOrder(out string error)
        {
            if (SelectedOrder == null)
            {
                error = "No Order Is Selected";
                return false;
            }
            else
            {
                if (WaiterClientModel.CancelOrder(SelectedOrder.Id))
                {
                    SelectedOrder.State = OrderState.NotRealized;
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


        public bool RealizeOrder(out string error)
        {
            if (SelectedOrder == null)
            {
                error = "No Order Is Selected";
                return false;
            }
            if (WaiterClientModel.RealizeOrder(SelectedOrder.Id))
            {
                SelectedOrder.State = OrderState.Realized;
                ArchivedOrdersViewModel.AddArchivedOrder(SelectedOrder);
                ListOfOrders.Remove(SelectedOrder);
                error = "";
                return true;
            }

            error = "Failed";
            return false;
            
        }
    }
}
