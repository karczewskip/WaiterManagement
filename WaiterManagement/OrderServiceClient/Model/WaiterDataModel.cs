using OrderServiceClient.Abstract;
using OrderServiceClient.WaiterDataAccessWCFService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderServiceClient.Model
{
    class WaiterDataModel : IWaiterDataModel
    {
        private IWaiterDataAccess _waiterDataAccess;
        private UserContext waiterUserContext;

        public WaiterDataModel(IWaiterDataAccess waiterDataAccess)
        {
            _waiterDataAccess = waiterDataAccess;
            waiterUserContext = null;
        }

        public void LogIn(string login, string password)
        {
            waiterUserContext = _waiterDataAccess.LogIn(login, ClassLib.DataStructures.HashClass.CreateFirstHash(password, login));
        }


        public bool IsLogged()
        {
            return waiterUserContext != null;
        }
    }
}
