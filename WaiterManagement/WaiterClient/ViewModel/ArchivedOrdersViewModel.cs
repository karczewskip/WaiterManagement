using System;
using System.Collections.Generic;
using WaiterClient.Abstract;
using System.Collections.ObjectModel;
using WaiterClient.WaiterDataAccessWCFService;

namespace WaiterClient.ViewModel
{
    /// <summary>
    /// Klasa odpowiedzialna za pokazywanie danych o starych zamówieniach
    /// </summary>
    public class ArchivedOrdersViewModel : IArchivedOrdersViewModel
    {
        private IWaiterClientModel WaiterClientModel;

        public IList<Order> ListOfOrders { get; set; }

        public ArchivedOrdersViewModel( IWaiterClientModel waiterClientModel)
        {
            WaiterClientModel = waiterClientModel;

            ListOfOrders = new ObservableCollection<Order>();
        }


        public void InitializeUser()
        {
            ListOfOrders.Clear();

            foreach ( var o in WaiterClientModel.GetPastOrders(0 , 20))
            {
                ListOfOrders.Add(o);
            }
        }


        public void AddArchivedOrder(Order selectedOrder)
        {
            ListOfOrders.Add(selectedOrder);
        }


        public void GetMore()
        {
            var pastList = WaiterClientModel.GetPastOrders(ListOfOrders.Count, ListOfOrders.Count + 20);
            foreach(var o in pastList )
            {
                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    ListOfOrders.Add(o);
                });
            }
        }
    }
}
