using Caliburn.Micro;
using ClassLib;
using OrderServiceClient.Abstract;
using OrderServiceClient.WaiterDataAccessWCFService;

namespace OrderServiceClient.ViewModels
{
    internal class MainWindowViewModel : Conductor<object>, IMainWindowViewModel
    {
        private readonly IDialogLogin _dialogLogin;
        private readonly IWaiterDataModel _waiterDataModel;
        private IConfirmDialogViewModel _confirmDialogViewModel;
        private readonly IOrderViewModelFactory _orderViewModelFactory;
        private readonly IConfirmDialogFactory _confirmDialogFactory;
        private IOrderDialog _orderDialog;

        public MainWindowViewModel(IOrderNotyficator orderNotyficator,
            IWaiterDataModel waiterDataModel, IDialogLogin dialogLogin, IOrderViewModelFactory orderViewModelFactory, IConfirmDialogFactory confirmDialogFactory)
        {
            _orderViewModelFactory = orderViewModelFactory;
            _confirmDialogFactory = confirmDialogFactory;
            _waiterDataModel = waiterDataModel;
            orderNotyficator.SetTarget(this);

            _dialogLogin = dialogLogin;
            _dialogLogin.SetMainWindowReference(this);
            
            ActivateItem(_dialogLogin);
        }

        public void LogIn()
        {
            if (_waiterDataModel.IsLogged())
                DeactivateItem(_dialogLogin, true);
        }

        public void ShowNewOrder(Order order)
        {
            _orderDialog = _orderViewModelFactory.GetOrderViewModel(order);
            ActivateItem(_orderDialog);
        }

        public bool GetConfirmFromWaiter(Order order)
        {
            _confirmDialogViewModel = _confirmDialogFactory.GetConfirmDialog(order);
            ActivateItem(_confirmDialogViewModel);
            var result = _confirmDialogViewModel.GetResult();
            DeactivateItem(_confirmDialogViewModel, true);

            return result;
        }

        public void ShowAcceptedOrder(Order order)
        {
            _orderDialog = _orderViewModelFactory.GetOrderViewModel(order);
            ActivateItem(_orderDialog);
        }

        public bool GetConfirmPayd()
        {
            _confirmDialogViewModel = _confirmDialogFactory.GetConfirmDialog(ApplicationResources.ConfirmPayingMessage);
            ActivateItem(_confirmDialogViewModel);
            var result = _confirmDialogViewModel.GetResult();
            DeactivateItem(_confirmDialogViewModel, true);

            return result;
        }

        public void CloseCurrentOrder()
        {
            DeactivateItem(_orderDialog, true);
        }
    }
}