using Caliburn.Micro;
using ClassLib.DbDataStructures;

namespace BarManager.Abstract
{
    public interface ITableManagerViewModel
    {
        BindableCollection<Table> Tables { get; set; }

        void CloseDialogs();
    }
}
