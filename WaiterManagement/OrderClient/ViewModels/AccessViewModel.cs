using Caliburn.Micro;
using OrderClient.Abstract;

namespace OrderClient.ViewModels
{
    public class AccessViewModel : Conductor<object>, IAccessViewModel
    {
        private readonly IMainWindowViewModel _mainWindowViewModel;
        private readonly IOrderDataModel _orderDataModel;
        private readonly IDialogRegister _dialogRegister;
        private readonly IDialogLogin _dialogLogin;

        public AccessViewModel(IMainWindowViewModel mainWindowViewModel, IOrderDataModel orderDataModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _orderDataModel = orderDataModel;

            _dialogRegister = new RegisterViewModel(mainWindowViewModel, orderDataModel);
            _dialogLogin = new LoginViewModel(_mainWindowViewModel, _orderDataModel);

            ActivateItem(_dialogRegister);
        }

        public void ShowLogIn()
        {
            ActivateItem(_dialogLogin);
        }

        public void CreateNewAccount()
        {
            ActivateItem(_dialogRegister);
        }

        
         
    }
}