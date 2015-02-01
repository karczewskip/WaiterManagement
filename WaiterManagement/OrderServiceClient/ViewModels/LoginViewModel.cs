using OrderServiceClient.Abstract;
using OrderServiceClient.Views;

namespace OrderServiceClient.ViewModels
{
    internal class LoginViewModel : IDialogLogin
    {
        private readonly IWaiterDataModel _waiterDataModel;
        private IMainWindowViewModel _mainWindow;

        public LoginViewModel(IWaiterDataModel waiterDataModel)
        {
            _waiterDataModel = waiterDataModel;
        }

        public string UserName { get; set; }

        public void LogIn(LoginView view)
        {
            _waiterDataModel.LogIn(UserName, view.PasswordB.Password);
            _mainWindow.LogIn();
        }

        public void SetMainWindowReference(IMainWindowViewModel mainWindowViewModel)
        {
            _mainWindow = mainWindowViewModel;
        }
    }
}