using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarManager.Abstract;
using System.Windows;
using System.ComponentModel;
using BarManager.Abstract.Model;
using BarManager.Abstract.ViewModel;
using BarManager.Messaging;

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
                Message.Show("Some Fields are empty");
                return;
            }

            int number;

            if (!int.TryParse(Number, out number))
            {
                Message.Show("Number is wrong");
                return;
            }

            if (TableManagerViewModel.Tables.Any(table => table.Number.Equals(number)))
            {
                Message.Show("There is table " + Number);
                return;
            }

            var addingTable = DataModel.AddTable(number, Description);
            if (addingTable != null)
            {
                TableManagerViewModel.Tables.Add(addingTable);
                TableManagerViewModel.CloseDialogs();

                return;
            }

            Message.Show("Failed");
            return;
        }


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
