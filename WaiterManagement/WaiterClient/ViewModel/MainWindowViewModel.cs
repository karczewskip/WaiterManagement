using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WaiterClient.Abstract;

namespace WaiterClient.ViewModel
{
    public class MainWindowViewModel : IMainWindowViewModel
    {
        private IOrderWindowViewModel OrderWindowViewModel;
        private IWaiterClientModel WaiterClientModel;

        public MainWindowViewModel(IWaiterClientModel waiterClientModel,IOrderWindowViewModel orderWindowViewModel)
        {
            WaiterClientModel = waiterClientModel;
            OrderWindowViewModel = orderWindowViewModel;
        }

        public bool LoginUser(string login, string password, out string error)
        {
            var waiter = WaiterClientModel.CheckUser(login, password);
            if( waiter != null )
            {
                OrderWindowViewModel.InitializeUser(waiter.Id);
                error = ""; 
                return true;
            }
            else
            {
                error = "User login or password was wrong";
                return false;
            }
        }
    }
}
