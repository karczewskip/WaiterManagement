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
        private IWaiterDataModel _waiterDataModel;

        public MainWindowViewModel(IOrderNotyficator _orderNotyficator, IWaiterDataModel waiterDataModel)
        {
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
    }
}
