using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarManager.Abstract
{
    public interface IEditTableViewModel
    {
        bool EditTable(out string error);

        void RefreshItem(ClassLib.DbDataStructures.Table table);
    }
}
