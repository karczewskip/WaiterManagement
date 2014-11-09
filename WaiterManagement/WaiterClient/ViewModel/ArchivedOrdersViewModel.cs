using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaiterClient.Abstract;
using ClassLib.DbDataStructures;
using System.Collections.ObjectModel;
using System.Threading;

namespace WaiterClient.ViewModel
{
    /// <summary>
    /// Klasa odpowiedzialna za pokazywanie danych o starych zamówieniach
    /// </summary>
    public class ArchivedOrdersViewModel : IArchivedOrdersViewModel
    {
        private IWaiterClientModel WaiterClientModel;
        private int WaiterId;

        public IList<Order> ListOfOrders { get; set; }

        public ArchivedOrdersViewModel( IWaiterClientModel waiterClientModel)
        {
            WaiterClientModel = waiterClientModel;

            ListOfOrders = new ObservableCollection<Order>();
        }


        public void InitializeUser(int id)
        {
            WaiterId = id;

            ListOfOrders.Clear();

            foreach ( var o in WaiterClientModel.GetPastOrders(id, 0 , 20))
            {
                ListOfOrders.Add(o);
            }
        }


        public void AddArchivedOrder(Order SelectedOrder)
        {
            ListOfOrders.Add(SelectedOrder);
        }


        public void GetMore()
        {
            var pastList = WaiterClientModel.GetPastOrders(WaiterId, ListOfOrders.Count, ListOfOrders.Count + 20);
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
