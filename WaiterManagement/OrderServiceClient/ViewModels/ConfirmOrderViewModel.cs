using System.Text;
using Caliburn.Micro;
using OrderServiceClient.WaiterDataAccessWCFService;

namespace OrderServiceClient.ViewModels
{
    internal class ConfirmOrderViewModel : Screen
    {
        private readonly Order _order;

        public ConfirmOrderViewModel(Order order)
        {
            _order = order;
        }

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

        public void Accept()
        {
            TryClose(true);
        }

        public void Reject()
        {
            TryClose(false);
        }
    }
}