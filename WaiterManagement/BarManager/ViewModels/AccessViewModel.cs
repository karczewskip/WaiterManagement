using BarManager.Abstract;
using BarManager.Abstract.ViewModel;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BarManager.ViewModels
{
    class AccessViewModel : Conductor<object>, IAccessViewModel
    {
        private IMainWindowViewModel _mainWindow;

        private ILoggerViewModel _loggerWindow;
        private IRegisterViewModel _registerWindow;

        public AccessViewModel(IMainWindowViewModel mainWindow, IBarDataModel barDataModel)
        {
            _mainWindow = mainWindow;
            _loggerWindow = new LoggerViewModel(this, barDataModel);
            _registerWindow = new RegisterViewModel(this, barDataModel);

        }

        public void ShowLogIn()
        {
            ActivateItem(_loggerWindow);
        }

        public void CreateNewAccount()
        {
            ActivateItem(_registerWindow);
        }



        public void LogIn()
        {
            _mainWindow.ReCheckLoggining();
        }
    }
}
