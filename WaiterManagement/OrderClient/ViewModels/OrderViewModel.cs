using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using OrderClient.Abstract;
using OrderClient.ClientDataAccessWCFService;

namespace OrderClient.ViewModels
{
    internal class OrderViewModel : Conductor<object>, IOrderViewModel, IDialogMainWindow
    {
        private readonly IDialogOrder _addItemDialog;
        private readonly ICurrentOrder _currentOrderDialog;
        private readonly IMainWindowViewModel _mainWindow;
        private readonly IOrderDataModel _orderDataModel;
        private readonly IWaitingViewModel _waitingDialog;
        private IPayingWindow _payingWindow;

        public OrderViewModel(IMainWindowViewModel mainWindow, IOrderDataModel orderDataModel)
        {
            _mainWindow = mainWindow;
            _currentOrderDialog = new CurrentOrderViewModel(this, orderDataModel);
            _addItemDialog = new AddItemViewModel(this, orderDataModel);
            _waitingDialog = new WaitingViewModel(this, orderDataModel);

            _orderDataModel = orderDataModel;

            _orderDataModel.SetTargetMessage(this);

            _orderDataModel.StartNewOrder();

            ActivateItem(_currentOrderDialog);
        }

        public bool CanAddCurrentOrder
        {
            get { return !_orderDataModel.IsEmpty(); }
        }

        public void CloseAddItemDialog()
        {
            _currentOrderDialog.RefreshOrder();
            ActivateItem(_currentOrderDialog);
            NotifyOfPropertyChange(() => CanAddCurrentOrder);
        }

        public void CheckIfIsPosibleToAddOrder()
        {
            NotifyOfPropertyChange(() => CanAddCurrentOrder);
        }

        public void SetOrderState(OrderState state)
        {
            _orderDataModel.SetOrderState(state);
            _waitingDialog.RefreshMessage();
        }

        public void ShowPayingWindow()
        {
            _payingWindow = new PayingViewModel(this, _orderDataModel);
            ActivateItem(_payingWindow);
        }

        public void CloseOrder()
        {
            _mainWindow.CloseOrder();
        }

        public void NotyfyOrderOnHold()
        {
            _orderDataModel.SetCurrentOrderOnHold();
            _waitingDialog.RefreshMessage();
        }

        public void AddCurrentOrder()
        {
            Task.Factory.StartNew(() => _orderDataModel.AddOrder());
            ActivateItem(_waitingDialog);
        }

        public void AddItem()
        {
            ActivateItem(_addItemDialog);
        }

        public void CancelOrder()
        {
            _mainWindow.CancelOrder();
        }
    }
}