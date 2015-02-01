using OrderServiceClient.WaiterDataAccessWCFService;
namespace OrderServiceClient.Abstract
{
    public interface IOrderViewModelFactory
    {

        IOrderDialog GetOrderViewModel(Order order);
    }
}