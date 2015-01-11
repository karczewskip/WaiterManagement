using Caliburn.Micro;
using OrderClient.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OrderClient.ViewModels
{
    class OrderViewModel : Conductor<object>,IOrderViewModel, IDialogMainWindow
    {
        private IMainWindowViewModel _mainWindow;
        private ICurrentOrder _currentOrderDialog;
        private IDialogOrder _addItemDialog;

        private IOrderDataModel _orderDataMoedl;

        public OrderViewModel(IMainWindowViewModel mainWindow, IOrderDataModel orderDataModel)
        {
            _mainWindow = mainWindow;
            _currentOrderDialog = new CurrentOrderViewModel(this, orderDataModel);
            _addItemDialog = new AddItemViewModel(this, orderDataModel);
            

            _orderDataMoedl = orderDataModel;

            _orderDataMoedl.StartNewOrder();

            ActivateItem(_currentOrderDialog);
        }

        public void AddCurrentOrder()
        {
            MessageBox.Show("Add current Order Clicked");
        }

        public bool CanAddCurrentOrder
        {
            get { return !_orderDataMoedl.IsEmpty(); }
        }

        public void AddItem()
        {
            ActivateItem(_addItemDialog);
        }

        public void CancelOrder()
        {
            _mainWindow.CancelOrder();
        }

        public void CloseAddItemDialog()
        {
            _currentOrderDialog.RefreshOrder();
            ActivateItem(_currentOrderDialog);
            NotifyOfPropertyChange(() => CanAddCurrentOrder);           
        }


        public void CheckIfIsPosibleToAddOrder()
        {
            NotifyOfPropertyChange(() => CanAddCurrentOrder);
        }
    }
}
