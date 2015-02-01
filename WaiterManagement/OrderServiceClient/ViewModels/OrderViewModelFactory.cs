using OrderServiceClient.Abstract;
using OrderServiceClient.WaiterDataAccessWCFService;

namespace OrderServiceClient.ViewModels
{
    internal class OrderViewModelFactory : IOrderViewModelFactory
    {
        private readonly IWaiterDataModel _waiterDataModel;

        public OrderViewModelFactory(IWaiterDataModel waiterDataModel)
        {
            _waiterDataModel = waiterDataModel;
        }

        public IOrderDialog GetOrderViewModel(Order order)
        {
            return new OrderViewModel(_waiterDataModel, order);
        }
    }
}