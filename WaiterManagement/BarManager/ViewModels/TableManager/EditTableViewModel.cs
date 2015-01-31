using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarManager.Abstract;
using System.ComponentModel;
using BarManager.ManagerDataAccessWCFService;
using System.Windows;
using BarManager.Abstract.Model;
using BarManager.Abstract.ViewModel;
using BarManager.Messaging;

namespace BarManager.ViewModels
{
    /// <summary>
    /// Klasa odpowiedzialna za edytowanie stolików
    /// </summary>
    public class EditTableViewModel : IEditTableViewModel, INotifyPropertyChanged
    {
        private ITableManagerViewModel TableManagerViewModel;
        private IBarDataModel DataModel;

        private Table Table;

        private string number;
        public string Number
        {
            get { return number; }
            set
            {
                number = value;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Number"));
                }
            }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Description"));
                }
            }
        }

        public IList<Table> ListOfTables { get { return TableManagerViewModel.Tables; } }

        public EditTableViewModel(IBarDataModel dataModel , ITableManagerViewModel tableManagerViewModel)
        {
            DataModel = dataModel;
            TableManagerViewModel = tableManagerViewModel;
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

            if (TableManagerViewModel.Tables.Any(table => (table.Number.Equals(checkedNumber) && Table.Id != table.Id)))
            {
                Message.Show("There is table " + checkedNumber);
                return;
            }

            var result = DataModel.EditTable(Table, checkedNumber, Description);

            if (result)
            {
                TableManagerViewModel.Tables.Refresh();
                TableManagerViewModel.CloseDialogs();
            }
            else
                Message.Show("Failed");

            return;
        }

        public void RefreshItem(Table table)
        {
            Table = table;

            Number = table.Number.ToString();
            Description = table.Description;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
