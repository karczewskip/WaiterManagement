using WaiterClient.WaiterDataAccessWCFService;

namespace WaiterClient.Abstract
{
    public interface IAddOrderViewModel
    {
        void AddItem(MenuItem menuItem);

        void AddObserverListView(System.Windows.Controls.ListView itemsListView);

        bool DeleteSelectedItem(out string error);

        bool AddOrder(out string error);

        void Clear();
    }
}
