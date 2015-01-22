using OrderServiceClient.Abstract;
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

        public OrderViewModel(IMainWindowViewModel mainWindow, IWaiterDataModel waiterDataModel)
        {
            _mainWindow = mainWindow;
            _waiterDataModel = waiterDataModel;
        }

        public void Accept()
        {
            _waiterDataModel.AcceptOrder();
        }
    }
}
