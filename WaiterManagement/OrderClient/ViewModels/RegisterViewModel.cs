using OrderClient.Abstract;
using OrderClient.Views;

namespace OrderClient.ViewModels
{
    internal class RegisterViewModel : IDialogRegister
    {
        private IMainWindowViewModel _mainWindow;
        private readonly IOrderDataModel _orderDataModel;

        public RegisterViewModel(IOrderDataModel orderDataModel)
        {
            _orderDataModel = orderDataModel;
        }

        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public void Register(RegisterView view)
        {
            _orderDataModel.AddClient(FirstName, LastName, UserName, view.PasswordB.Password);
            _orderDataModel.Login(UserName,view.PasswordB.Password);

            if (_orderDataModel.IsLogged())
                _mainWindow.LogIn();
        }

        public void SetMainWindowReference(IMainWindowViewModel mainWindowViewModel)
        {
            _mainWindow = mainWindowViewModel;
        }
    }
}