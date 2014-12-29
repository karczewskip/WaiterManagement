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
        private IDialogOrder _currentOrderDialog;
        private IDialogOrder _addItemDialog;

        public OrderViewModel(IMainWindowViewModel mainWindow)
        {
            _mainWindow = mainWindow;
            _addItemDialog = new AddItemViewModel(this);
            _currentOrderDialog = new CurrentOrderViewModel(this);
            ActivateItem(_currentOrderDialog);
        }

        public void AddCurrentOrder()
        {
            MessageBox.Show("Add current Order Clicked");
        }

        public bool CanAddCurrentOrder
        {
            get { return false; }
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
            ActivateItem(_currentOrderDialog);
            NotifyOfPropertyChange(() => CanAddCurrentOrder);           
        }
    }
}
