using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarManager.Abstract
{
    public interface IAddWaiterViewModel
    {
        bool AddWaiter(out string error);

        void Clear();
    }
}
