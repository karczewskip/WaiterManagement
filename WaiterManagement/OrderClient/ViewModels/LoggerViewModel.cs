using ClassLib.DataStructures;
using OrderClient.Abstract;
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

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = HashClass.CreateFirstHash(value, _userName);
            }
        }

        public LoggerViewModel(IMainWindowViewModel mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public void LogIn()
        {
            MessageBox.Show("User Name == " + _userName + ", password == " + _password);
            _mainWindow.LogIn();
        }

        public void PasswordChanged(PasswordBox p)
        {
            Password = p.Password;
        }

        
    }
}
