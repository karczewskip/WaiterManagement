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

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        public LoggerViewModel(IMainWindowViewModel mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public void LogIn(LoggerView view)
        {
            //MessageBox.Show(view.PasswordB.Password);
            //MessageBox.Show("User Name == " + _userName + ", password == " + HashClass.CreateFirstHash(view.PasswordB.Password, _userName));
            _mainWindow.LogIn();
        }

    }
}
