using ClassLib.DbDataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaiterClient.Abstract;

namespace WaiterClient.ViewModel
{
    public class AddOrderViewModel : IAddOrderViewModel
    {
        IOrderWindowViewModel OrderWindowViewModel;

        public Table SelectedTable { get; set; }
        public IList<Table> ListOfTables { get { return OrderWindowViewModel.ListOfTables; } }

        public AddOrderViewModel(IOrderWindowViewModel orderWindowViewModel)
        {
            OrderWindowViewModel = orderWindowViewModel;
        }

    }
}
