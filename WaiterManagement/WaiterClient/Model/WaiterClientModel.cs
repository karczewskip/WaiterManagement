using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaiterClient.Abstract;
using DataAccess;
using ClassLib.DbDataStructures;
using System.Windows;

namespace WaiterClient.Model
{
    public class WaiterClientModel : IWaiterClientModel
    {
        private IWaiterDataAccess WaiterDataAccess;

        public WaiterClientModel(IWaiterDataAccess waiterDataAccess)
        {
            WaiterDataAccess = waiterDataAccess;
        }

        public WaiterContext CheckUser(string login, string password)
        {
            try
            {
                return WaiterDataAccess.LogIn(login, password);
            }
            catch
            {
                return null;
            }
        }
    }
}
