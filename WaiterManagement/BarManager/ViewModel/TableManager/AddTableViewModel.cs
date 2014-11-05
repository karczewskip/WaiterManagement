using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarManager.Abstract;
using System.Windows;
using System.ComponentModel;

namespace BarManager.ViewModel
{
    public class AddTableViewModel: IAddTableViewModel, INotifyPropertyChanged
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

        public bool AddTable(out string error)
        {
            if (string.IsNullOrEmpty(NumberString) || string.IsNullOrEmpty(TableDescription))
            {
                error = "Some Fields are empty";
                return false;
            }

            int Number;

            if (!int.TryParse(NumberString, out Number))
            {
                error = "Number is wrong";
                return false;
            }

            if (TableManagerViewModel.ListOfTables.Any(table => table.Number.Equals(Number)))
            {
                error = "There is table " + Number;
                return false;
            }

            var AddingTable = DataModel.AddTable(Number, TableDescription);
            if (AddingTable!= null)
            {
                TableManagerViewModel.ListOfTables.Add(AddingTable);
                error = "";
                return true;
            }

            error = "Failed";
            return false;
        }


        public void Clear()
        {
            NumberString = "";
            TableDescription = "";

            if (null != this.PropertyChanged)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("NumberString"));
                PropertyChanged(this, new PropertyChangedEventArgs("TableDescription"));
                
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
