using BarManager.ManagerDataAccessWCFService;

namespace BarManager.Abstract.ViewModel
{
    public interface IEditWaiterViewModel
    {

        void ChangeWaiter();

        void RefreshItem(UserContext SelectedWaiter);
    }
}
