using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarManager.Abstract
{
    public interface IEditWaiterViewModel
    {
        void RefreshItem(ClassLib.DbDataStructures.WaiterContext Waiter);

        bool EditWaiter();
    }
}
