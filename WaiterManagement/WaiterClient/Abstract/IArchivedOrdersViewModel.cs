using WaiterClient.WaiterDataAccessWCFService;

namespace WaiterClient.Abstract
{
    public interface IArchivedOrdersViewModel
    {
        void InitializeUser();

        void AddArchivedOrder(Order selectedOrder);

        void GetMore();
    }
}
