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
        private IDialogMainWindow _dialogOrderWindow;

        public void AddNewOrder()
        {
            _dialogOrderWindow = new OrderViewModel(this);
            ActivateItem(_dialogOrderWindow);
        }

        public void CancelOrder()
        {
            DeactivateItem(_dialogOrderWindow, true);
        }
    }
}
