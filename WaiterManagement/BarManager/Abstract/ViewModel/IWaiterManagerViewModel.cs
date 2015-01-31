using BarManager.ManagerDataAccessWCFService;
using Caliburn.Micro;

namespace BarManager.Abstract.ViewModel
{
    public interface IWaiterManagerViewModel
    {
        BindableCollection<UserContext> Waiters { get; set; }

        void DeleteWaiter();

        void CloseDialogs();

        void RefreshData();
    }
}
