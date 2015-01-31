using BarManager.Abstract.Model;
using BarManager.Abstract.ViewModel;
using BarManager.Views;

namespace BarManager.ViewModels
{
    internal class LoggerViewModel : ILoggerViewModel
    {
        private readonly ICredentialDataModel _credentialDataModel;
        private IAccessViewModel _accessViewModel;

        public LoggerViewModel(ICredentialDataModel credentialDataModel)
        {
            _credentialDataModel = credentialDataModel;
        }

        public string UserName { get; set; }

        public void SetParentWindow(IAccessViewModel accessViewModel)
        {
            _accessViewModel = accessViewModel;
        }

        public void LogIn(LoggerView view)
        {
            _credentialDataModel.LogIn(view.UserName.Text, view.PasswordB.Password);
            _accessViewModel.LogIn();
        }
    }
}