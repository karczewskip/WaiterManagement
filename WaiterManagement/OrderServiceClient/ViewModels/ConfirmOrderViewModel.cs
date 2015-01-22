using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderServiceClient.ViewModels
{
    class ConfirmOrderViewModel : Screen
    {
        private WaiterDataAccessWCFService.Order order;

        public ConfirmOrderViewModel(WaiterDataAccessWCFService.Order order)
        {
            // TODO: Complete member initialization
            this.order = order;
        }

        public string OrderDetails
        {
            get {
                var content = new StringBuilder();
                foreach(var o in order.MenuItems)
                {
                    content.Append(o.MenuItem.Name + "(" + o.Quantity + "),");
                }
                return "Client:  " + order.Client.Login + 
                ",\nTable: " + order.Table.Description + 
                ",\nContent: " + content.ToString(); }
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
