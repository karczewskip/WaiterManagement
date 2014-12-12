
namespace BarManager.Abstract
{
    public interface IEditTableViewModel
    {
        void ChangeTable();

        void RefreshItem(ClassLib.DbDataStructures.Table table);
    }
}
