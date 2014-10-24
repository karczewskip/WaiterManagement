using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarManager.Abstract;
using System.Windows;

namespace BarManager.ViewModel
{
    public class AddTableViewModel: IAddTableViewModel
    {
        private IBarDataModel DataModel;
        private ITableManagerViewModel TableManagerViewModel;

        public string NumberString { get; set; }
        public string TableDescription { get; set; }

        public AddTableViewModel(IBarDataModel dataModel,ITableManagerViewModel tableManagerViewmodel)
        {
            DataModel = dataModel;
            TableManagerViewModel = tableManagerViewmodel;
        }

        public bool AddTable()
        {
            if (string.IsNullOrEmpty(NumberString) || string.IsNullOrEmpty(TableDescription))
            {
                MessageBox.Show("Some Fields are empty");
                return false;
            }

            int Number;

            if (!int.TryParse(NumberString, out Number))
            {
                MessageBox.Show("Number is wrong");
                return false;
            }

            if (TableManagerViewModel.ListOfTables.Any(table => table.Number.Equals(Number)))
            {
                MessageBox.Show("There is table " + Number);
                return false;
            }

            var AddingTable = DataModel.AddTable(Number, TableDescription);
            if (AddingTable!= null)
            {
                TableManagerViewModel.ListOfTables.Add(AddingTable);
                return true;
            }

            return false;
        }
    }
}
