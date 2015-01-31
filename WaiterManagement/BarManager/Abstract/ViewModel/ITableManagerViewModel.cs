using BarManager.ManagerDataAccessWCFService;
using Caliburn.Micro;

namespace BarManager.Abstract.ViewModel
{
    public interface ITableManagerViewModel
    {
        BindableCollection<Table> Tables { get; set; }

        void CloseDialogs();

        void RefreshData();
    }
}
