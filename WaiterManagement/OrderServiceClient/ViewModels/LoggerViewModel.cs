using OrderServiceClient.Abstract;
using OrderServiceClient.Views;

namespace OrderServiceClient.ViewModels
{
    internal class LoggerViewModel : IDialogLogin
    {
        private readonly IMainWindowViewModel _mainWindow;
        private readonly IWaiterDataModel _waiterDataModel;

        public LoggerViewModel(IMainWindowViewModel mainWindow, IWaiterDataModel waiterDataModel)
        {
            _mainWindow = mainWindow;
            _waiterDataModel = waiterDataModel;
        }

        public string UserName { get; set; }

        public void LogIn(LoggerView view)
        {
            _waiterDataModel.LogIn(UserName, view.PasswordB.Password);
            _mainWindow.LogIn();
        }
    }
}