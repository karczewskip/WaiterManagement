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

        public void DeleteSelectedItem()
        {
            if (SelectedWaiter == null)
                MessageBox.Show("No Item Is Sellected");
            else
            {
                if (DataModel.DeleteWaiter(SelectedWaiter.Id))
                    ListOfWaiters.Remove(SelectedWaiter);
                else
                    MessageBox.Show("Failed");
            }
        }
    }
}
