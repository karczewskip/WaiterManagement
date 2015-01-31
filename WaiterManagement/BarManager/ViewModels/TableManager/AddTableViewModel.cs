using System.ComponentModel;
using System.Linq;
using BarManager.Abstract.Model;
using BarManager.Abstract.ViewModel;
using BarManager.Messaging;

namespace BarManager.ViewModels
{
    /// <summary>
    ///     Klasa odpowiedzialna za dodawanie stolików
    /// </summary>
    public class AddTableViewModel : IAddTableViewModel, INotifyPropertyChanged
    {
        private readonly ITableDataModel _tableDataModel;
        private readonly ITableManagerViewModel _tableManagerViewModel;

        public AddTableViewModel(ITableDataModel tableDataModel, ITableManagerViewModel tableManagerViewmodel)
        {
            _tableDataModel = tableDataModel;
            _tableManagerViewModel = tableManagerViewmodel;
        }

        public string Number { get; set; }
        public string Description { get; set; }

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

            if (_tableManagerViewModel.Tables.Any(table => table.Number.Equals(number)))
            {
                Message.Show("There is table " + Number);
                return;
            }

            var addingTable = _tableDataModel.AddTable(number, Description);
            if (addingTable != null)
            {
                _tableManagerViewModel.Tables.Add(addingTable);
                _tableManagerViewModel.CloseDialogs();

                return;
            }

            Message.Show("Failed");
        }

        public void Clear()
        {
            Number = "";
            Description = "";

            if (null != PropertyChanged)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("NumberString"));
                PropertyChanged(this, new PropertyChangedEventArgs("TableDescription"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}