using ClassLib.DataStructures;
using OrderClient.Abstract;
using OrderClient.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace OrderClient.ViewModels
{
    class LoggerViewModel : IDialogLogin
    {
        private IMainWindowViewModel _mainWindow;
        private IOrderDataModel _orderDataModel;

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        public LoggerViewModel(IMainWindowViewModel mainWindow,IOrderDataModel orderDataModel)
        {
            _mainWindow = mainWindow;
            _orderDataModel = orderDataModel;
        }

        public void LogIn(LoggerView view)
        {
            _orderDataModel.LogIn(_userName, view.PasswordB.Password);
            _mainWindow.LogIn();
        }
        
    }
}
