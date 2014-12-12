using System.Windows;
using BarManager.Abstract;
using Caliburn.Micro;
using ClassLib.DbDataStructures;

namespace BarManager.ViewModels
{
    /// <summary>
    /// Klasa odpowiedzialna za przetwarzanie danych kelnerów
    /// </summary>
    public class WaiterManagerViewModel : Conductor<object>, IWaiterManagerViewModel 
    {
        private IBarDataModel DataModel;
        private IAddWaiterViewModel _addWaiterViewModel;
        private IEditWaiterViewModel _editWaiterViewModel;

        public WaiterContext SelectedWaiter { get; set; }

        private BindableCollection<WaiterContext> _waiters;
        public BindableCollection<WaiterContext> Waiters
        {
            get { return _waiters; }
            set
            {
                _waiters = value;
                NotifyOfPropertyChange(() => Waiters);
            }
        }

        public WaiterManagerViewModel(IBarDataModel dataModel)
        {
            DataModel = dataModel;

            Waiters = new BindableCollection<WaiterContext>();

            InitializeData();
        }

        private void InitializeData()
        {
            Waiters = new BindableCollection<WaiterContext>(DataModel.GetAllWaiters());
        }

        public void DeleteWaiter()
        {
            if (SelectedWaiter == null)
            {
                //error = "No Item Is Selected";
                return;
            }

            if (DataModel.DeleteWaiter(SelectedWaiter.Id))
            {
                Waiters.Remove(SelectedWaiter);
                //error = "";
                return;
            }

            //error = "Failed";
            return;
        }

        public void AddWaiter()
        {
            _addWaiterViewModel = IoC.Get<IAddWaiterViewModel>();
            _addWaiterViewModel.Clear();
            ActivateItem(_addWaiterViewModel);
        }

        public void CloseDialogs()
        {
            CloseAddWaiterDialog();
            CloseEditWaiterDialog();
        }

        private void CloseAddWaiterDialog()
        {
            DeactivateItem(_addWaiterViewModel,true);
        }

        private void CloseEditWaiterDialog()
        {
            DeactivateItem(_editWaiterViewModel, true);
        }

        public void EditWaiter()
        {
            if (SelectedWaiter == null)
            {
                //TODO: Add massege
                return;
            }

            _editWaiterViewModel = IoC.Get<IEditWaiterViewModel>();
            _editWaiterViewModel.RefreshItem(SelectedWaiter);
            ActivateItem(_editWaiterViewModel);
        }


        
    }
}
