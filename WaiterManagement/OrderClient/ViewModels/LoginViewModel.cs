using OrderClient.Abstract;
using OrderClient.Views;

namespace OrderClient.ViewModels
{
    public class LoginViewModel : IDialogLogin
    {
        private readonly IMainWindowViewModel _mainWindow;
        private readonly IOrderDataModel _orderDataModel;

        public LoginViewModel(IMainWindowViewModel mainWindow, IOrderDataModel orderDataModel)
        {
            _mainWindow = mainWindow;
            _orderDataModel = orderDataModel;
        }

        public string UserName { get; set; }

        public void LogIn(LoginView view)
        {
            _orderDataModel.Login(UserName, view.PasswordB.Password);

            if (_orderDataModel.IsLogged())
                _mainWindow.LogIn();
        }
    }
}