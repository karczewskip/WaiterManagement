using BarManager.Abstract.Model;
using BarManager.Abstract.ViewModel;
using BarManager.ManagerDataAccessWCFService;
using Caliburn.Micro;
using Message = BarManager.Messaging.Message;

namespace BarManager.ViewModels
{
    /// <summary>
    ///     Klasa odpowiedzialna za przetwarzanie danych stolików
    /// </summary>
    public class TableManagerViewModel : Conductor<object>, ITableManagerViewModel
    {
        private readonly ITableDataModel _tableDataModel;
        private IAddTableViewModel _addTableViewModel;
        private IEditTableViewModel _editTableViewModel;
        private BindableCollection<Table> _tables;

        public TableManagerViewModel(ITableDataModel tableDataModel)
        {
            _tableDataModel = tableDataModel;

            Tables = new BindableCollection<Table>();
        }

        public Table SelectedTable { get; set; }

        public BindableCollection<Table> Tables
        {
            get { return _tables; }
            set
            {
                _tables = value;
                NotifyOfPropertyChange(() => Tables);
            }
        }

        public void CloseDialogs()
        {
            CloseAddTableDialog();
            CloseEditTableDialog();
        }

        public void RefreshData()
        {
            InitializeData();
        }

        private void InitializeData()
        {
            Tables = new BindableCollection<Table>(_tableDataModel.GetAllTables());
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
                Message.Show("No Item Is Selected");
                return;
            }

            if (_tableDataModel.DeleteTable(SelectedTable.Id))
            {
                Tables.Remove(SelectedTable);
                return;
            }

            Message.Show("Failed");
        }

        private void CloseAddTableDialog()
        {
            DeactivateItem(_addTableViewModel, true);
        }

        private void CloseEditTableDialog()
        {
            DeactivateItem(_editTableViewModel, true);
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