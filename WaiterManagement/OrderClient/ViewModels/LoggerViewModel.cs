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

        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        private bool _registrationMode;
        public bool RegistrationMode 
        {
            get { return _registrationMode; }
            set { _registrationMode = value; }
        }

        public LoggerViewModel(IMainWindowViewModel mainWindow,IOrderDataModel orderDataModel)
        {
            _mainWindow = mainWindow;
            _orderDataModel = orderDataModel;
        }

        public void LogIn(LoggerView view)
        {
            if (_registrationMode)
                _orderDataModel.AddClient(FirstName, LastName, _userName, view.PasswordB.Password);
            else
                _orderDataModel.Login(_userName, view.PasswordB.Password);

            _mainWindow.LogIn();
        }
        
    }
}
