using OrderClient.Abstract;

namespace OrderClient.ViewModels
{
    internal class PayingViewModel : IPayingWindow
    {
        private readonly IOrderDataModel _orderDataModel;
        private IOrderViewModel _orderViewModel;

        public PayingViewModel(IOrderDataModel orderDataModel)
        {
            _orderDataModel = orderDataModel;
        }

        public void SetOrderWindowReference(OrderViewModel orderViewModel)
        {
            _orderViewModel = orderViewModel;
        }

        public void Pay()
        {
            _orderDataModel.Pay();
            _orderViewModel.CloseOrder();
        }
    }
}