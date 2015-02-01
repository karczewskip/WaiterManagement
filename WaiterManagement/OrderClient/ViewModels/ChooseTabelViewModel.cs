using System.Collections.Generic;
using Caliburn.Micro;
using OrderClient.Abstract;
using OrderClient.ClientDataAccessWCFService;

namespace OrderClient.ViewModels
{
    internal class ChooseTabelViewModel : PropertyChangedBase, IChooseTabelViewModel
    {
        private readonly IOrderDataModel _orderDataModel;
        private IMainWindowViewModel _mainWindowViewModel;

        public ChooseTabelViewModel(IOrderDataModel orderDataModel)
        {
            _orderDataModel = orderDataModel;
        }

        public Table SelectedTable { get; set; }
        public IList<Table> Tables { get; set; }

        public void InitializeData()
        {
            Tables = _orderDataModel.GetTables();
            NotifyOfPropertyChange(() => Tables);
        }

        public void SetMainWindowReference(IMainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
        }

        public void ChooseTable()
        {
            if (SelectedTable == null) return;

            _orderDataModel.SetTableId(SelectedTable.Id);
            _mainWindowViewModel.StartGettingOrders();
        }
    }
}