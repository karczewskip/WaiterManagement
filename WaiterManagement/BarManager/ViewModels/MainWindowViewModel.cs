using System.Windows;
using BarManager.Abstract;
using Caliburn.Micro;

namespace BarManager.ViewModels
{
    public class MainWindowViewModel : Conductor<object>
    {
        private readonly IMenuManagerViewModel _menuManagerViewModel;
        private readonly IWaiterManagerViewModel _waiterManagerViewModel;
        private readonly ITableManagerViewModel _tableManagerViewModel;

        public MainWindowViewModel(IMenuManagerViewModel menuManagerViewModel, IWaiterManagerViewModel waiterManagerViewModel, ITableManagerViewModel tableManagerViewModel)
        {
            _menuManagerViewModel = menuManagerViewModel;
            _waiterManagerViewModel = waiterManagerViewModel;
            _tableManagerViewModel = tableManagerViewModel;
        }


        public void MenuManager() 
        {
            CloseAllDialogs();
            ActivateItem(_menuManagerViewModel );
        }

        public void WaiterManager()
        {
            CloseAllDialogs();
            ActivateItem(_waiterManagerViewModel);
        }

        public void TableManager()
        {
            CloseAllDialogs();
            ActivateItem(_tableManagerViewModel);
        }

        public void Close()
        {
            Application.Current.Shutdown();
        }

        private void CloseAllDialogs()
        {
            _waiterManagerViewModel.CloseDialogs();
            _tableManagerViewModel.CloseDialogs();
        }
    }
}