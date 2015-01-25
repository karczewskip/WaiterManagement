using Caliburn.Micro;
using OrderClient.Abstract;

namespace OrderClient.ViewModels
{
    internal class MainWindowViewModel : Conductor<object>, IMainWindowViewModel
    {
        private readonly IChooseTabelViewModel _chooseTabelViewModel;
        private readonly IAccessViewModel _accessViewModel;
        private readonly IOrderDataModel _orderDataModel;
        private IDialogMainWindow _dialogOrderWindow;

        public MainWindowViewModel(IOrderDataModel orderDataModel)
        {
            _accessViewModel = new AccessViewModel(this, orderDataModel);
            _chooseTabelViewModel = new ChooseTabelViewModel(this, orderDataModel);
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
            _dialogOrderWindow = new OrderViewModel(this, _orderDataModel);
            ActivateItem(_dialogOrderWindow);
        }
    }
}