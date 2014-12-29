using BarManager.Abstract;
using BarManager.Messaging;
using System.ComponentModel;
using System.Linq;

namespace BarManager.ViewModels
{
    /// <summary>
    /// Klasa odpowiedzialna za dodawanie klientów
    /// </summary>
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

            if (WaiterManagerViewModel.Waiters.Any(cat => cat.Login.Equals(Login)))
            {
                Message.Show("There is login named: " + Login);
                return;
            }

            var addingWaiter = DataModel.AddWaiter(Login, FirstName, LastName, Password);
            if (addingWaiter != null)
            {
                WaiterManagerViewModel.Waiters.Add(addingWaiter);

                WaiterManagerViewModel.CloseDialogs();
                return;
            }

            Message.Show("Failed");
            return;
           
        }
    }
}
