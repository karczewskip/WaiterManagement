using BarManager.Abstract.Model;
using BarManager.Abstract.ViewModel;
using BarManager.Views;

namespace BarManager.ViewModels
{
    internal class LoggerViewModel : ILoggerViewModel
    {
        private readonly IBarDataModel _barDataModel;
        private IAccessViewModel _accessViewModel;

        public LoggerViewModel(IBarDataModel barDataModel)
        {
            _barDataModel = barDataModel;
        }

        public string UserName { get; set; }

        public void SetParentWindow(IAccessViewModel accessViewModel)
        {
            _accessViewModel = accessViewModel;
        }

        public void LogIn(LoggerView view)
        {
            _barDataModel.LogIn(view.UserName.Text, view.PasswordB.Password);
            _accessViewModel.LogIn();
        }
    }
}