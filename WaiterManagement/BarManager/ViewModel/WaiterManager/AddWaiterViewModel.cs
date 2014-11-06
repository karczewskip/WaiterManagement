using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarManager.Abstract;
using System.Windows;
using System.ComponentModel;

namespace BarManager.ViewModel
{
    public class AddWaiterViewModel : IAddWaiterViewModel , INotifyPropertyChanged
    {
        private IBarDataModel DataModel;
        private IWaiterManagerViewModel WaiterManagerViewModel;

        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }

        public AddWaiterViewModel(IBarDataModel dataModel, IWaiterManagerViewModel waiterManagerViewModel)
        {
            DataModel = dataModel;
            WaiterManagerViewModel = waiterManagerViewModel;
        }

        public bool AddWaiter(out string error)
        {
            if (string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName) || string.IsNullOrEmpty(Password))
            {
                error = "Some Fields are empty";
                return false;
            }

            if (WaiterManagerViewModel.ListOfWaiters.Any(cat => cat.Login.Equals(Login)))
            {
                error = "There is login named: " + Login;
                return false;
            }

            var AddingWaiter = DataModel.AddWaiter(Login, FirstName, LastName, Password);
            if (AddingWaiter != null)
            {
                WaiterManagerViewModel.ListOfWaiters.Add(AddingWaiter);
                error = "";
                return true;
            }

            error = "Failed";

            return false;
        }


        public void Clear()
        {
            Login = "";
            FirstName = "";
            LastName = "";
            Password = "";

            if (null != this.PropertyChanged)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("Login"));
                PropertyChanged(this, new PropertyChangedEventArgs("FirstName"));
                PropertyChanged(this, new PropertyChangedEventArgs("LastName"));
                PropertyChanged(this, new PropertyChangedEventArgs("Password"));
                
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
