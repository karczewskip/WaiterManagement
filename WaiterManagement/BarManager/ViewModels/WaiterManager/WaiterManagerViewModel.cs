using System.Windows;
using BarManager.Abstract;
using BarManager.Abstract.Model;
using BarManager.Abstract.ViewModel;
using Caliburn.Micro;
using BarManager.ManagerDataAccessWCFService;

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

        public UserContext SelectedWaiter { get; set; }

        private BindableCollection<UserContext> _waiters;
        public BindableCollection<UserContext> Waiters
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

            Waiters = new BindableCollection<UserContext>();

            InitializeData();
        }

        private void InitializeData()
        {
            if (!DataModel.IsLogged())
                return;

            Waiters = new BindableCollection<UserContext>(DataModel.GetAllWaiters());
        }

        public void DeleteWaiter()
        {
            if (SelectedWaiter == null)
            {
                Messaging.Message.Show("No Item Is Selected");
                return;
            }

            if (DataModel.DeleteWaiter(SelectedWaiter.Id))
            {
                Waiters.Remove(SelectedWaiter);
                return;
            }

            Messaging.Message.Show("Failed");
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
                return;
            }

            _editWaiterViewModel = IoC.Get<IEditWaiterViewModel>();
            _editWaiterViewModel.RefreshItem(SelectedWaiter);
            ActivateItem(_editWaiterViewModel);
        }

        public void RefreshData()
        {
            InitializeData();
        }
    }
}
