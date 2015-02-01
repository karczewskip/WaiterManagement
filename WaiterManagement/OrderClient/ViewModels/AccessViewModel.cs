using Caliburn.Micro;
using OrderClient.Abstract;

namespace OrderClient.ViewModels
{
    public class AccessViewModel : Conductor<object>, IAccessViewModel
    {
        private readonly IDialogRegister _dialogRegister;
        private readonly IDialogLogin _dialogLogin;

        public AccessViewModel(IDialogRegister dialogRegister, IDialogLogin dialogLogin)
        {
            _dialogRegister = dialogRegister;
            _dialogLogin = dialogLogin;

            ActivateItem(_dialogLogin);
        }

        public void ShowLogIn()
        {
            ActivateItem(_dialogLogin);
        }

        public void CreateNewAccount()
        {
            ActivateItem(_dialogRegister);
        }

        public void SetMainWindowReference(IMainWindowViewModel mainWindowViewModel)
        {
            _dialogLogin.SetMainWindowReference(mainWindowViewModel);
            _dialogRegister.SetMainWindowReference(mainWindowViewModel);
        }
    }
}