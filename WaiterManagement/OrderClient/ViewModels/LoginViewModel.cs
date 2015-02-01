using OrderClient.Abstract;
using OrderClient.Views;

namespace OrderClient.ViewModels
{
    public class LoginViewModel : IDialogLogin
    {
        private IMainWindowViewModel _mainWindow;
        private readonly IOrderDataModel _orderDataModel;

        public LoginViewModel(IOrderDataModel orderDataModel)
        {
            _orderDataModel = orderDataModel;
        }

        public string UserName { get; set; }

        public void LogIn(LoginView view)
        {
            _orderDataModel.Login(UserName, view.PasswordB.Password);

            if (_orderDataModel.IsLogged())
                _mainWindow.LogIn();
        }

        public void SetMainWindowReference(IMainWindowViewModel mainWindowViewModel)
        {
            _mainWindow = mainWindowViewModel;
        }
    }
}