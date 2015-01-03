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

        public MainWindowViewModel(IOrderNotyficator _orderNotyficator)
        {
            _orderNotyficator.SetTarget(this);

            _dialogLogin = new LoggerViewModel(this);
            ActivateItem(_dialogLogin);

        }

        public void LogIn()
        {
            MessageBox.Show("Loggining");
            DeactivateItem(_dialogLogin, true);
        }
    }
}
