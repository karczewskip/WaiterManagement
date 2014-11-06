using ClassLib.DbDataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaiterClient.Abstract
{
    public interface IAddOrderViewModel
    {

        void AddItem(MenuItem menuItem);

        void AddObserverListView(System.Windows.Controls.ListView ItemsListView);

        bool DeleteSelectedItem(out string error);

        bool AddOrder(out string error);

        void Clear();
    }
}
