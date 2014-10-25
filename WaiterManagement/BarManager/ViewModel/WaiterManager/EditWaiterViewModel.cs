using BarManager.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLib.DbDataStructures;
using System.Windows;

namespace BarManager.ViewModel
{
    public class EditWaiterViewModel : IEditWaiterViewModel, INotifyPropertyChanged
    {
        private IWaiterManagerViewModel WaiterManagerViewModel;
        private IBarDataModel DataModel;

        private WaiterContext Waiter;

        private string login;
        public string Login 
        {
            get { return login; }
            set
            {
                login = value;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Login"));
                }
            }
        }

        private string firstName;
        public string FirstName 
        {
            get { return firstName; }
            set
            {
                firstName = value;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("FirstName"));
                }
            }
        }

        private string lastName;
        public string LastName 
        {
            get { return lastName; }
            set
            {
                lastName = value;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("LastName"));
                }
            }
        }

        private string password;
        public string Password 
        {
            get { return password; }
            set
            {
                password = value;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Password"));
                }
            }
        }

        public IList<WaiterContext> ListOfWaiters { get { return WaiterManagerViewModel.ListOfWaiters; } }

        public EditWaiterViewModel(IBarDataModel dataModel, IWaiterManagerViewModel waiterManagerViewModel)
        {
            DataModel = dataModel;
            WaiterManagerViewModel = waiterManagerViewModel;
        }

        public void RefreshItem(ClassLib.DbDataStructures.WaiterContext waiter)
        {
            Waiter = waiter;

            Login = waiter.Login;
            FirstName = waiter.FirstName;
            LastName = waiter.LastName;
            Password = waiter.Password;
        }

        public bool EditWaiter()
        {
            if (string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName) || string.IsNullOrEmpty(Password))
            {
                MessageBox.Show("Some Fields are empty");
                return false;
            }

            if (WaiterManagerViewModel.ListOfWaiters.Any(waiter => (waiter.Login.Equals(Login) && waiter.Id != Waiter.Id)))
            {
                MessageBox.Show("There is waiter named: " + Login);
                return false;
            }

            var result = DataModel.EditWaiter(Waiter, Login, FirstName, LastName, Password);

            return result;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
