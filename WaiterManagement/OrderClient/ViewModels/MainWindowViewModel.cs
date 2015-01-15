using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using OrderClient.Abstract;

namespace OrderClient.ViewModels
{
    class MainWindowViewModel : Conductor<object>, IMainWindowViewModel
    {
        private IDialogLogin _dialogLogin;
        private IDialogMainWindow _dialogOrderWindow;
        private IOrderDataModel _orderDataModel;
        private IChooseTabelViewModel _chooseTabelViewModel;

        public MainWindowViewModel(IOrderDataModel orderDataModel)
        {
            _dialogLogin = new LoggerViewModel(this, orderDataModel);
            _chooseTabelViewModel = new ChooseTabelViewModel(this, orderDataModel);
            _orderDataModel = orderDataModel;
            ActivateItem(_dialogLogin);
        }

        public void AddNewOrder()
        {
            _dialogOrderWindow = new OrderViewModel(this, _orderDataModel);
            ActivateItem(_dialogOrderWindow);
        }

        public void CancelOrder()
        {
            DeactivateItem(_dialogOrderWindow, true);
        }

        public void LogIn()
        {
            DeactivateItem(_dialogLogin, true);
            _chooseTabelViewModel.InitializeData();
            ActivateItem(_chooseTabelViewModel);
        }


        public void StartGettingOrders()
        {
            DeactivateItem(_chooseTabelViewModel, true);
        }
    }
}
