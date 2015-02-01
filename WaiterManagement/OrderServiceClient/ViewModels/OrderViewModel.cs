using System.Text;
using Caliburn.Micro;
using OrderServiceClient.Abstract;
using OrderServiceClient.WaiterDataAccessWCFService;

namespace OrderServiceClient.ViewModels
{
    internal class OrderViewModel : Screen, IOrderDialog
    {
        private readonly Order _order;
        private readonly IWaiterDataModel _waiterDataModel;

        public OrderViewModel(IWaiterDataModel waiterDataModel, Order order)
        {
            _waiterDataModel = waiterDataModel;
            _order = order;
            CanReady = true;
        }

        public bool CanReady { get; set; }

        public string OrderDetails
        {
            get
            {
                var content = new StringBuilder();
                foreach (var o in _order.MenuItems)
                {
                    content.Append(o.MenuItem.Name + "(" + o.Quantity + "),");
                }
                return "Client:  " + _order.Client.Login +
                       ",\nTable: " + _order.Table.Description +
                       ",\nContent: " + content;
            }
        }

        public void Ready()
        {
            _waiterDataModel.NotifyReady(_order);
            CanReady = false;
            NotifyOfPropertyChange(() => CanReady);
        }
    }
}