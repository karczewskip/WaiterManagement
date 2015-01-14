using OrderServiceClient.Abstract;
using OrderServiceClient.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OrderServiceClient.ViewModels
{
    class LoggerViewModel : IDialogLogin
    {
        private IMainWindowViewModel _mainWindow;
        private IWaiterDataModel _waiterDataModel;

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        public LoggerViewModel(IMainWindowViewModel mainWindow, IWaiterDataModel waiterDataModel)
        {
            _mainWindow = mainWindow;
            _waiterDataModel = waiterDataModel;
        }

        public void LogIn(LoggerView view)
        {
            _waiterDataModel.LogIn(_userName, view.PasswordB.Password);
            _mainWindow.LogIn();
        }

    }
}
