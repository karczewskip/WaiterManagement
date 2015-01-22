using Caliburn.Micro;
using OrderServiceClient.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OrderServiceClient.ViewModels
{
    class MainWindowViewModel : Conductor<object>, IMainWindowViewModel
    {
        private IDialogLogin _dialogLogin;
        private IOrderDialog _orderDialog;
        private IWaiterDataModel _waiterDataModel;
        private IWindowManager _windowManager;

        public MainWindowViewModel(IWindowManager windowManager, IOrderNotyficator _orderNotyficator, IWaiterDataModel waiterDataModel)
        {
            _windowManager = windowManager;
            _waiterDataModel = waiterDataModel;
            _orderNotyficator.SetTarget(this);

            _dialogLogin = new LoggerViewModel(this, _waiterDataModel);
            ActivateItem(_dialogLogin);

        }

        public void LogIn()
        {
            if(_waiterDataModel.IsLogged())
                DeactivateItem(_dialogLogin, true);
        }


        public void ShowNewOrder(WaiterDataAccessWCFService.Order order)
        {
            _orderDialog = new OrderViewModel(this, _waiterDataModel);
            ActivateItem(_orderDialog);
        }


        public bool GetConfirmFromWaiter(WaiterDataAccessWCFService.Order order)
        {
            var result = _windowManager.ShowDialog(new ConfirmOrderViewModel(order));

            if (result.HasValue)
                return result.Value;

            return false;
        }
    }
}
