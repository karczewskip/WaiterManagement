
using BarManager.ManagerDataAccessWCFService;

namespace BarManager.Abstract.ViewModel
{
    public interface IEditTableViewModel
    {
        void ChangeTable();

        void RefreshItem(Table table);
    }
}
