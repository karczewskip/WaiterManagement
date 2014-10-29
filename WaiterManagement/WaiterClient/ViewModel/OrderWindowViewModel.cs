using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaiterClient.Abstract;
using DataAccess;

namespace WaiterClient.ViewModel
{
    public class OrderWindowViewModel : IOrderWindowViewModel
    {
        private IWaiterDataAccess WaiterDataAccess;

        private int WaiterId;

        public OrderWindowViewModel(IWaiterDataAccess waiterDataAccess)
        {
            WaiterDataAccess = waiterDataAccess;
        }

        public void LogOut()
        {
            WaiterDataAccess.LogOut(WaiterId);
        }


        public void InitializeUser(int id)
        {
            WaiterId = id;
        }
    }
}
