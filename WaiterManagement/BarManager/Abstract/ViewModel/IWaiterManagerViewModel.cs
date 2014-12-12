using Caliburn.Micro;
using ClassLib.DbDataStructures;

namespace BarManager.Abstract
{
    public interface IWaiterManagerViewModel
    {
        BindableCollection<WaiterContext> Waiters { get; set; }

        void DeleteWaiter();

        void CloseDialogs();
    }
}
