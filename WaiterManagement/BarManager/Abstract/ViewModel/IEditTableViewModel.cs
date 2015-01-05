
using BarManager.ManagerDataAccessWCFService;
namespace BarManager.Abstract
{
    public interface IEditTableViewModel
    {
        void ChangeTable();

        void RefreshItem(Table table);
    }
}
