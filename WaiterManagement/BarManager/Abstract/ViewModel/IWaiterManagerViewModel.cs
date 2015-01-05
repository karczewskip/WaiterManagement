using Caliburn.Micro;
using BarManager.ManagerDataAccessWCFService;

namespace BarManager.Abstract
{
    public interface IWaiterManagerViewModel
    {
        BindableCollection<UserContext> Waiters { get; set; }

        void DeleteWaiter();

        void CloseDialogs();
    }
}
