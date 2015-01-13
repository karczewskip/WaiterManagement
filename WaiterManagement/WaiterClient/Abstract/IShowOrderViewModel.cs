using WaiterClient.WaiterDataAccessWCFService;

namespace WaiterClient.Abstract
{
    public interface IShowOrderViewModel
    {
        void RefreshOrder(Order order);
    }
}
