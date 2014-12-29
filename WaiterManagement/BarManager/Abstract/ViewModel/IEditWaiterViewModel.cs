
using ClassLib.DbDataStructures;
namespace BarManager.Abstract
{
    public interface IEditWaiterViewModel
    {

        void ChangeWaiter();

        void RefreshItem(UserContext SelectedWaiter);
    }
}
