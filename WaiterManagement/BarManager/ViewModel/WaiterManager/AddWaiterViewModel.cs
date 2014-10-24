using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarManager.Abstract;
using System.Windows;

namespace BarManager.ViewModel
{
    public class AddWaiterViewModel : IAddWaiterViewModel
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

        public bool AddWaiter()
        {
            if (string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName) || string.IsNullOrEmpty(Login))
            {
                MessageBox.Show("Some Fields are empty");
                return false;
            }

            if (WaiterManagerViewModel.ListOfWaiters.Any(cat => cat.Login.Equals(Login)))
            {
                MessageBox.Show("There is login named: " + Login);
                return false;
            }

            var AddingWaiter = DataModel.AddWaiter(Login, FirstName, LastName, Password);
            if (AddingWaiter != null)
            {
                WaiterManagerViewModel.ListOfWaiters.Add(AddingWaiter);
                return true;
            }

            return false;
        }
    }
}
