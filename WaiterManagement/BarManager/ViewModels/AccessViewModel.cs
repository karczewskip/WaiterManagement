using BarManager.Abstract;
using BarManager.Abstract.Model;
using BarManager.Abstract.ViewModel;
using Caliburn.Micro;

namespace BarManager.ViewModels
{
    internal class AccessViewModel : Conductor<object>, IAccessViewModel
    {
        private readonly ILoggerViewModel _loggerWindow;
        private readonly IRegisterViewModel _registerWindow;

        private IMainWindowViewModel _mainWindow;

        public AccessViewModel(ILoggerViewModel loggerViewModel, IRegisterViewModel registerViewModel)
        {
            _loggerWindow = loggerViewModel;
            _loggerWindow.SetParentWindow(this);

            _registerWindow = registerViewModel;
            _registerWindow.SetParentWindow(this);
        }

        public void ShowLogIn()
        {
            ActivateItem(_loggerWindow);
        }

        public void LogIn()
        {
            _mainWindow.ReCheckLoggining();
        }

        public void CreateNewAccount()
        {
            ActivateItem(_registerWindow);
        }

        public void SetMainWindow(IMainWindowViewModel mainWindowViewModel)
        {
            _mainWindow = mainWindowViewModel;
        }
    }
}