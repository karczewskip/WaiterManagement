using Caliburn.Micro;
using OrderClient.Abstract;
using OrderClient.ClientDataAccessWCFService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OrderClient.ViewModels
{
    class ChooseTabelViewModel : PropertyChangedBase, IChooseTabelViewModel
    {
        private IMainWindowViewModel _mainWindowViewModel;
        private IOrderDataModel _orderDataModel;

        public Table SelectedTable { get; set; }
        public IList<Table> Tables { get; set; }

        public ChooseTabelViewModel(IMainWindowViewModel mainWindowViewModel, IOrderDataModel orderDataModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _orderDataModel = orderDataModel;
        }

        public void ChooseTable()
        {
            if (SelectedTable != null)
            {
                _orderDataModel.SetTableId(SelectedTable.Id);
                _mainWindowViewModel.StartGettingOrders();
            }
        }

        public void InitializeData()
        {
            Tables = _orderDataModel.GetTables();
            NotifyOfPropertyChange(() => Tables);
        }
    }
}
