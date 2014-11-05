using BarManager.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLib.DbDataStructures;
using System.Collections.ObjectModel;
using System.Windows;

namespace BarManager.ViewModel
{
    public class WaiterManagerViewModel : IWaiterManagerViewModel
    {
        private IBarDataModel DataModel;

        public WaiterContext SelectedWaiter { get; set; }

        public IList<WaiterContext> ListOfWaiters { get; set; }

        public WaiterManagerViewModel(IBarDataModel dataModel)
        {
            DataModel = dataModel;

            ListOfWaiters = new ObservableCollection<WaiterContext>();

            InitializeData();
        }

        private void InitializeData()
        {
            ListOfWaiters = new ObservableCollection<WaiterContext>(DataModel.GetAllWaiters());
        }

        public bool DeleteSelectedItem(out string error)
        {
            if (SelectedWaiter == null)
            {
                error = "No Item Is Selected";
                return false;
            }
            else
            {
                if (DataModel.DeleteWaiter(SelectedWaiter.Id))
                {
                    ListOfWaiters.Remove(SelectedWaiter);
                    error = "";
                    return true;
                }
                else
                {
                    error = "Failed";
                    return false;
                }
            }
        }
    }
}
