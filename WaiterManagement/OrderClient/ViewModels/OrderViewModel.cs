using System.Threading.Tasks;
using Caliburn.Micro;
using OrderClient.Abstract;
using OrderClient.ClientDataAccessWCFService;

namespace OrderClient.ViewModels
{
    internal class OrderViewModel : Conductor<object>, IOrderViewModel
    {
        private readonly IDialogAddingItem _addItemDialog;
        private readonly ICurrentOrder _currentOrderDialog;
        private IMainWindowViewModel _mainWindow;
        private readonly IOrderDataModel _orderDataModel;
        private readonly IWaitingViewModel _waitingDialog;
        private readonly IPayingWindow _payingWindow;

        public OrderViewModel(IOrderDataModel orderDataModel, ICurrentOrder currentOrder, IDialogAddingItem addDialogOrder, IWaitingViewModel waitingViewModel, IPayingWindow payingWindow)
        {
            _currentOrderDialog = currentOrder;
            _currentOrderDialog.SetOrderWindowReference(this);

            _addItemDialog = addDialogOrder;
            _addItemDialog.SetOrderWindowReference(this);

            _waitingDialog = waitingViewModel;

            _payingWindow = payingWindow;
            _payingWindow.SetOrderWindowReference(this);

            _orderDataModel = orderDataModel;

            _orderDataModel.SetTargetMessage(this);

            _orderDataModel.StartNewOrder();

            IsAddingElements = false;
            IsProcessingOrder = false;

            ActivateItem(_currentOrderDialog);
        }

        public bool IsAddingElements { get; set; }
        public bool IsProcessingOrder { get; set; }

        public bool CanAddCurrentOrder
        {
            get { return !IsProcessingOrder && !IsAddingElements && !_orderDataModel.IsEmpty(); }
        }

        public bool CanAddItem
        {
            get { return !IsProcessingOrder && !IsAddingElements; }
        }

        public bool CanCancelOrder
        {
            get { return !IsProcessingOrder; }
        }

        public void CloseAddItemDialog()
        {
            _currentOrderDialog.RefreshOrder();
            ActivateItem(_currentOrderDialog);

            IsAddingElements = false;

            RefreshAvailableOptions();
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

        private void RefreshAvailableOptions()
        {
            NotifyOfPropertyChange(() => CanAddCurrentOrder);
            NotifyOfPropertyChange(() => CanAddItem);
            NotifyOfPropertyChange(() => CanCancelOrder);
        }

        public void AddCurrentOrder()
        {
            Task.Factory.StartNew(() => _orderDataModel.AddOrder());
            IsProcessingOrder = true;
            RefreshAvailableOptions();
            ActivateItem(_waitingDialog);
        }

        public void AddItem()
        {
            IsAddingElements = true;
            RefreshAvailableOptions();

            ActivateItem(_addItemDialog);
        }

        public void CancelOrder()
        {
            _mainWindow.CancelOrder();
        }


        public void SetMainWindowReference(IMainWindowViewModel mainWindowViewModel)
        {
            _mainWindow = mainWindowViewModel;
        }
    }
}