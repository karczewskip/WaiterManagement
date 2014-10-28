using ClassLib.DbDataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarManager.Abstract
{
    public interface IWaiterManagerViewModel
    {
        IList<WaiterContext> ListOfWaiters { get; set; }

        bool DeleteSelectedItem(out string error);
    }
}
