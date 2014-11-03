using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaiterClient.Abstract;
using ClassLib.DbDataStructures;
using System.Collections.ObjectModel;

namespace WaiterClient.ViewModel
{
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

            

            //TODO! Dodać poczatkowe 20 past orders
        }
    }
}
