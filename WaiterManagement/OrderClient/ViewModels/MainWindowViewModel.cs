using Caliburn.Micro;
using OrderClient.Abstract;

namespace OrderClient.ViewModels
{
    internal class MainWindowViewModel : Conductor<object>, IMainWindowViewModel
    {
        private readonly IChooseTabelViewModel _chooseTabelViewModel;
        private readonly IOrderViewModelFactory _orderViewModelFactory;
        private readonly IAccessViewModel _accessViewModel;
        private readonly IOrderDataModel _orderDataModel;
        private IOrderViewModel _dialogOrderWindow;

        public MainWindowViewModel(IOrderDataModel orderDataModel, IAccessViewModel accessViewModel, IChooseTabelViewModel chooseTabelViewModel, IOrderViewModelFactory orderViewModelFactory)
        {
            _accessViewModel = accessViewModel;
            _accessViewModel.SetMainWindowReference(this);
            _chooseTabelViewModel = chooseTabelViewModel;
            _orderViewModelFactory = orderViewModelFactory;
            _orderViewModelFactory.SetMainWindowReference(this);
            _orderDataModel = orderDataModel;

            ActivateItem(_accessViewModel);
        }

        public void CancelOrder()
        {
            DeactivateItem(_dialogOrderWindow, true);
        }

        public void LogIn()
        {
            DeactivateItem(_accessViewModel, true);
            _chooseTabelViewModel.InitializeData();
            _chooseTabelViewModel.SetMainWindowReference(this);
            ActivateItem(_chooseTabelViewModel);
        }

        public void StartGettingOrders()
        {
            DeactivateItem(_chooseTabelViewModel, true);
        }

        public void CloseOrder()
        {
            DeactivateItem(_dialogOrderWindow, true);
        }

        public void AddNewOrder()
        {
            _dialogOrderWindow = _orderViewModelFactory.GetOrderViewModel();
            ActivateItem(_dialogOrderWindow);
        }

        protected override void OnDeactivate(bool close)
        {
            _orderDataModel.LogOut();
            base.OnDeactivate(close);
        }
    }
}