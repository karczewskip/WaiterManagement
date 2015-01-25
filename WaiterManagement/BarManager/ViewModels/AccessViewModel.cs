using BarManager.Abstract;
using BarManager.Abstract.ViewModel;
using Caliburn.Micro;

namespace BarManager.ViewModels
{
    internal class AccessViewModel : Conductor<object>, IAccessViewModel
    {
        private readonly ILoggerViewModel _loggerWindow;
        private readonly IMainWindowViewModel _mainWindow;
        private readonly IRegisterViewModel _registerWindow;

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

        public void LogIn()
        {
            _mainWindow.ReCheckLoggining();
        }

        public void CreateNewAccount()
        {
            ActivateItem(_registerWindow);
        }
    }
}