using Caliburn.Micro;
using BarManager.ManagerDataAccessWCFService;

namespace BarManager.Abstract
{
    public interface ITableManagerViewModel
    {
        BindableCollection<Table> Tables { get; set; }

        void CloseDialogs();
    }
}
