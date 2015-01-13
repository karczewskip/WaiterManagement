using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarManager.Abstract;
using Caliburn.Micro;
using BarManager.ManagerDataAccessWCFService;
using System.Collections.ObjectModel;
using System.Windows;

namespace BarManager.ViewModels
{
    /// <summary>
    /// Klasa odpowiedzialna za przetwarzanie danych stolików
    /// </summary>
    public class TableManagerViewModel :Conductor<object>, ITableManagerViewModel
    {
        private IBarDataModel DataModel;
        private IAddTableViewModel _addTableViewModel;
        private IEditTableViewModel _editTableViewModel;

        public Table SelectedTable { get; set; }

        private BindableCollection<Table> _tables;
        public BindableCollection<Table> Tables
        {
            get { return _tables; }
            set
            {
                _tables = value;
                NotifyOfPropertyChange(() => Tables);
            }
        }

        public TableManagerViewModel(IBarDataModel dataModel)
        {
            DataModel = dataModel;

            Tables = new BindableCollection<Table>();

            InitializeData();
        }

        private void InitializeData()
        {
            //Tables = new BindableCollection<Table>(DataModel.GetAllTables());
        }

        public void AddTable()
        {
            _addTableViewModel = IoC.Get<IAddTableViewModel>();
            _addTableViewModel.Clear();
            ActivateItem(_addTableViewModel);
        }

        public void DeleteTable()
        {
            if (SelectedTable == null)
            {
                Messaging.Message.Show("No Item Is Selected");
                return;
            }

            if (DataModel.DeleteTable(SelectedTable.Id))
            {
                Tables.Remove(SelectedTable);
                return;
            }

            Messaging.Message.Show("Failed");
            return;
        }


        public void CloseDialogs()
        {
            CloseAddTableDialog();
            CloseEditTableDialog();
        }

        private void CloseAddTableDialog()
        {
            DeactivateItem(_addTableViewModel,true);
        }

        private void CloseEditTableDialog()
        {
            DeactivateItem(_editTableViewModel,true);
        }

        public void EditTable()
        {
            if (SelectedTable == null)
            {
                //TODO: Add massege
                return;
            }

            _editTableViewModel = IoC.Get<IEditTableViewModel>();
            _editTableViewModel.RefreshItem(SelectedTable);
            ActivateItem(_editTableViewModel);
        }

        
    }
}
