using OrderServiceClient.Abstract;
using OrderServiceClient.WaiterDataAccessWCFService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OrderServiceClient.ViewModels
{
    class OrderViewModel : IOrderDialog
    {
        private IMainWindowViewModel _mainWindow;
        private IWaiterDataModel _waiterDataModel;
        private Order _order;

        public OrderViewModel(IMainWindowViewModel mainWindow, IWaiterDataModel waiterDataModel, Order order)
        {
            _mainWindow = mainWindow;
            _waiterDataModel = waiterDataModel;
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
                ",\nContent: " + content.ToString();
            }
        }

        public void Ready()
        {
            _waiterDataModel.NotifyReady(_order);
        }
    }
}
