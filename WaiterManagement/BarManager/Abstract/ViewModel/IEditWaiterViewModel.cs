
namespace BarManager.Abstract
{
    public interface IEditWaiterViewModel
    {
        void RefreshItem(ClassLib.DbDataStructures.WaiterContext Waiter);

        void ChangeWaiter();
    }
}
