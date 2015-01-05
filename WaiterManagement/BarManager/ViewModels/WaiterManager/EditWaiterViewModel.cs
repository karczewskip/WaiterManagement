using BarManager.Abstract;
using BarManager.Messaging;
using BarManager.ManagerDataAccessWCFService;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BarManager.ViewModels
{
    /// <summary>
    /// Klasa odpowiedzialna za edytowanie klientów
    /// </summary>
    public class EditWaiterViewModel : IEditWaiterViewModel, INotifyPropertyChanged
    {
        private IWaiterManagerViewModel WaiterManagerViewModel;
        private IBarDataModel DataModel;

        private UserContext Waiter;

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

        public IList<UserContext> ListOfWaiters { get { return WaiterManagerViewModel.Waiters; } }

        public EditWaiterViewModel(IBarDataModel dataModel, IWaiterManagerViewModel waiterManagerViewModel)
        {
            DataModel = dataModel;
            WaiterManagerViewModel = waiterManagerViewModel;
        }

        public void RefreshItem(UserContext waiter)
        {
            Waiter = waiter;

            Login = waiter.Login;
            FirstName = waiter.FirstName;
            LastName = waiter.LastName;
            //Password = waiter.Password; 
            //TODO: Password
        }

        public void ChangeWaiter()
        {
            if (string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName) || string.IsNullOrEmpty(Password))
            {
                Message.Show("Some Fields are empty");
                return;
            }

            if (WaiterManagerViewModel.Waiters.Any(waiter => (waiter.Login.Equals(Login) && waiter.Id != Waiter.Id)))
            {
                Message.Show("There is waiter named: " + Login);
                return;
            }

            var result = DataModel.EditWaiter(Waiter, Login, FirstName, LastName, Password);

            if (result)
            {
                WaiterManagerViewModel.Waiters.Refresh();
                WaiterManagerViewModel.CloseDialogs();
            }
            else
                Message.Show("Failed");

            return;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
