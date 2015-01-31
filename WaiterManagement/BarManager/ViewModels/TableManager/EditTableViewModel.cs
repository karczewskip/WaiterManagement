using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using BarManager.Abstract.Model;
using BarManager.Abstract.ViewModel;
using BarManager.ManagerDataAccessWCFService;
using BarManager.Messaging;

namespace BarManager.ViewModels
{
    /// <summary>
    ///     Klasa odpowiedzialna za edytowanie stolików
    /// </summary>
    public class EditTableViewModel : IEditTableViewModel, INotifyPropertyChanged
    {
        private readonly ITableDataModel _tableDataModel;
        private readonly ITableManagerViewModel TableManagerViewModel;
        private string _number;
        private Table _table;
        private string description;

        public EditTableViewModel(ITableDataModel tableDataModel, ITableManagerViewModel tableManagerViewModel)
        {
            _tableDataModel = tableDataModel;
            TableManagerViewModel = tableManagerViewModel;
        }

        public string Number
        {
            get { return _number; }
            set
            {
                _number = value;
                if (null != PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Number"));
                }
            }
        }

        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                if (null != PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Description"));
                }
            }
        }

        public IList<Table> ListOfTables
        {
            get { return TableManagerViewModel.Tables; }
        }

        public void ChangeTable()
        {
            if (string.IsNullOrEmpty(Number) || string.IsNullOrEmpty(Description))
            {
                Message.Show("Some Fields are empty");
                return;
            }

            int checkedNumber;

            if (!int.TryParse(Number, out checkedNumber))
            {
                Message.Show("Number is wrong");
                return;
            }

            if (TableManagerViewModel.Tables.Any(table => (table.Number.Equals(checkedNumber) && _table.Id != table.Id)))
            {
                Message.Show("There is table " + checkedNumber);
                return;
            }

            var result = _tableDataModel.EditTable(_table, checkedNumber, Description);

            if (result)
            {
                TableManagerViewModel.Tables.Refresh();
                TableManagerViewModel.CloseDialogs();
            }
            else
                Message.Show("Failed");
        }

        public void RefreshItem(Table table)
        {
            _table = table;

            Number = table.Number.ToString();
            Description = table.Description;
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}