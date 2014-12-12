using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarManager.Abstract;
using System.Windows;
using System.ComponentModel;

namespace BarManager.ViewModels
{
    /// <summary>
    /// Klasa odpowiedzialna za dodawanie stolików
    /// </summary>
    public class AddTableViewModel: IAddTableViewModel, INotifyPropertyChanged
    {
        private IBarDataModel DataModel;
        private ITableManagerViewModel TableManagerViewModel;

        public string Number { get; set; }
        public string Description { get; set; }

        public AddTableViewModel(IBarDataModel dataModel,ITableManagerViewModel tableManagerViewmodel)
        {
            DataModel = dataModel;
            TableManagerViewModel = tableManagerViewmodel;
        }

        public void AddTable()
        {
            if (string.IsNullOrEmpty(Number) || string.IsNullOrEmpty(Description))
            {
                //TODO: Messaging
                //error = "Some Fields are empty";
                return;
            }

            int number;

            if (!int.TryParse(Number, out number))
            {
                //error = "Number is wrong";
                return;
            }

            if (TableManagerViewModel.Tables.Any(table => table.Number.Equals(number)))
            {
                //error = "There is table " + Number;
                return;
            }

            var addingTable = DataModel.AddTable(number, Description);
            if (addingTable != null)
            {
                TableManagerViewModel.Tables.Add(addingTable);
                TableManagerViewModel.CloseDialogs();

                return;
            }

            //error = "Failed";
            return;
        }

        //public bool AddTable(out string error)
        //{
        //    if (string.IsNullOrEmpty(Number) || string.IsNullOrEmpty(Description))
        //    {
        //        error = "Some Fields are empty";
        //        return false;
        //    }

        //    int number;

        //    if (!int.TryParse(Number, out number))
        //    {
        //        error = "Number is wrong";
        //        return false;
        //    }

        //    if (TableManagerViewModel.Tables.Any(table => table.Number.Equals(number)))
        //    {
        //        error = "There is table " + number;
        //        return false;
        //    }

        //    var addingTable = DataModel.AddTable(number, Description);
        //    if (addingTable!= null)
        //    {
        //        TableManagerViewModel.Tables.Add(addingTable);
        //        error = "";
        //        return true;
        //    }

        //    error = "Failed";
        //    return false;
        //}


        public void Clear()
        {
            Number = "";
            Description = "";

            if (null != this.PropertyChanged)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("NumberString"));
                PropertyChanged(this, new PropertyChangedEventArgs("TableDescription"));
                
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
