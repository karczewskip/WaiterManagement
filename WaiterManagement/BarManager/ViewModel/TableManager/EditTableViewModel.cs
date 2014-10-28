using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarManager.Abstract;
using System.ComponentModel;
using ClassLib.DbDataStructures;
using System.Windows;

namespace BarManager.ViewModel
{
    public class EditTableViewModel : IEditTableViewModel, INotifyPropertyChanged
    {
        private ITableManagerViewModel TableManagerViewModel;
        private IBarDataModel DataModel;

        private Table Table;

        private string numberString;
        public string NumberString
        {
            get { return numberString; }
            set
            {
                numberString = value;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("NumberString"));
                }
            }
        }

        private string tableDescription;
        public string TableDescription
        {
            get { return tableDescription; }
            set
            {
                tableDescription = value;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("TableDescription"));
                }
            }
        }

        public IList<Table> ListOfTables { get { return TableManagerViewModel.ListOfTables; } }

        public EditTableViewModel(IBarDataModel dataModel , ITableManagerViewModel tableManagerViewModel)
        {
            DataModel = dataModel;
            TableManagerViewModel = tableManagerViewModel;
        }

        public bool EditTable(out string error)
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

            if (TableManagerViewModel.ListOfTables.Any(table => (table.Number.Equals(Number) && Table.Id != table.Id)))
            {
                error = "There is table " + Number;
                return false;
            }

            var result = DataModel.EditTable(Table, Number, TableDescription);

            if (result)
                error = "";
            else
                error = "Failed";

            return result;
        }

        public void RefreshItem(ClassLib.DbDataStructures.Table table)
        {
            Table = table;

            NumberString = table.Number.ToString();
            TableDescription = table.Description;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
