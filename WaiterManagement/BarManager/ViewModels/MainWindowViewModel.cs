using System.Windows;
using BarManager.Abstract;
using BarManager.Abstract.Model;
using Caliburn.Micro;
using BarManager.Abstract.ViewModel;

namespace BarManager.ViewModels
{
    public class MainWindowViewModel : Conductor<object>, IMainWindowViewModel
    {
        private readonly IMenuManagerViewModel _menuManagerViewModel;
        private readonly IWaiterManagerViewModel _waiterManagerViewModel;
        private readonly ITableManagerViewModel _tableManagerViewModel;
        private readonly ICredentialDataModel _credentialDataModel;

        public MainWindowViewModel(IMenuManagerViewModel menuManagerViewModel, IWaiterManagerViewModel waiterManagerViewModel, ITableManagerViewModel tableManagerViewModel, IAccessViewModel accessViewModel, ICredentialDataModel credentialDataModel)
        {
            _credentialDataModel = credentialDataModel;

            _menuManagerViewModel = menuManagerViewModel;
            _waiterManagerViewModel = waiterManagerViewModel;
            _tableManagerViewModel = tableManagerViewModel;

            accessViewModel.SetMainWindow(this);

            ActivateItem(accessViewModel);
        }

        public void MenuManager() 
        {
            CloseAllDialogs();
            ActivateItem(_menuManagerViewModel );
        }

        public bool CanMenuManager
        {
            get { return IsLogged(); }
        }

        private bool IsLogged()
        {
            return _credentialDataModel.IsLogged();
        }

        public void WaiterManager()
        {
            CloseAllDialogs();
            ActivateItem(_waiterManagerViewModel);
        }

        public bool CanWaiterManager
        {
            get { return IsLogged(); }
        }

        public void TableManager()
        {
            CloseAllDialogs();
            ActivateItem(_tableManagerViewModel);
        }

        public bool CanTableManager
        {
            get { return IsLogged(); }
        }

        public void Close()
        {
            Application.Current.Shutdown();
        }

        private void CloseAllDialogs()
        {
            _menuManagerViewModel.CloseDialogs();
            _waiterManagerViewModel.CloseDialogs();
            _tableManagerViewModel.CloseDialogs();
        }

        public void ReCheckLoggining()
        {
            if(IsLogged())
            {
                NotifyOfPropertyChange(() => CanMenuManager);
                NotifyOfPropertyChange(() => CanWaiterManager);
                NotifyOfPropertyChange(() => CanTableManager);

                RefreshData();

                MenuManager();
            }
        }

        private void RefreshData()
        {
            _menuManagerViewModel.RefreshData();
            _waiterManagerViewModel.RefreshData();
            _tableManagerViewModel.RefreshData();

        }
    }
}