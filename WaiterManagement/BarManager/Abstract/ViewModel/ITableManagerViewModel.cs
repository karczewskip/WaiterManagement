using ClassLib.DbDataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarManager.Abstract
{
    public interface ITableManagerViewModel
    {
        IList<Table> ListOfTables { get; set; }

        bool DeleteSelectedItem(out string error);
    }
}
