using BarManager.Messaging;
using System.ComponentModel;
using System.Linq;
using BarManager.Abstract.Model;
using BarManager.Abstract.ViewModel;

namespace BarManager.ViewModels
{
    /// <summary>
    /// Klasa odpowiedzialna za dodawanie klientów
    /// </summary>
    public class AddWaiterViewModel : IAddWaiterViewModel , INotifyPropertyChanged
    {
        private readonly IWaiterDataModel _waiterDataModel;
        private readonly IWaiterManagerViewModel _waiterManagerViewModel;

        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }

        public AddWaiterViewModel(IWaiterDataModel waiterDataModel, IWaiterManagerViewModel waiterManagerViewModel)
        {
            _waiterDataModel = waiterDataModel;
            _waiterManagerViewModel = waiterManagerViewModel;
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

        public void AddWaiter()
        {
            if (string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName) || string.IsNullOrEmpty(Password))
            {
                Message.Show("Some Fields are empty");
                return;
            }

            if (_waiterManagerViewModel.Waiters.Any(cat => cat.Login.Equals(Login)))
            {
                Message.Show("There is login named: " + Login);
                return;
            }

            var addingWaiter = _waiterDataModel.AddWaiter(Login, FirstName, LastName, Password);
            if (addingWaiter != null)
            {
                _waiterManagerViewModel.Waiters.Add(addingWaiter);

                _waiterManagerViewModel.CloseDialogs();
                return;
            }

            Message.Show("Failed");
        }
    }
}
