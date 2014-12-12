using Caliburn.Micro;
using ClassLib.DbDataStructures;

namespace BarManager.Abstract
{
    public interface ITableManagerViewModel
    {
        BindableCollection<Table> Tables { get; set; }

        bool DeleteSelectedItem(out string error);

        void CloseDialogs();
    }
}
