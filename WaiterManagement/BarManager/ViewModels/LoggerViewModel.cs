using BarManager.Abstract;
using BarManager.Abstract.ViewModel;
using BarManager.Views;
using ClassLib.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BarManager.ViewModels
{
    class LoggerViewModel : ILoggerViewModel
    {
        private IAccessViewModel _accessViewModel;
        private IBarDataModel _barDataModel;

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        public LoggerViewModel(IAccessViewModel accessViewModel, IBarDataModel barDataModel )
        {
            _accessViewModel = accessViewModel;
            _barDataModel = barDataModel;
        }

        public void LogIn(LoggerView view)
        {
            //MessageBox.Show(view.PasswordB.Password);
            //MessageBox.Show("User Name == " + _userName + ", password == " + HashClass.CreateFirstHash(view.PasswordB.Password, _userName));
            _barDataModel.LogIn(view.UserName.Text, view.PasswordB.Password);
            _accessViewModel.LogIn();
        }
    }
}
