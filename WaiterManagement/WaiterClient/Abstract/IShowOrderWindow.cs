using WaiterClient.WaiterDataAccessWCFService;

namespace WaiterClient.Abstract
{
    public interface IShowOrderWindow
    {
        bool? ShowDialog(Order order);
    }
}
