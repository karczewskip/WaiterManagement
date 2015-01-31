using System;
using System.Collections.Generic;
using System.Linq;
using BarManager.Abstract;
using BarManager.Abstract.Model;
using BarManager.ManagerDataAccessWCFService;

namespace BarManager.Model
{
    public class WaiterDataModel : IWaiterDataModel
    {
        private readonly IManagerDataAccess _managerDataAccess;

        public WaiterDataModel(IManagerDataAccess managerDataAccess)
        {
            _managerDataAccess = managerDataAccess;
        }

        public IList<UserContext> GetAllWaiters()
        {
            try
            {
                return _managerDataAccess.GetWaiters().ToList();
            }
            catch
            {
                throw new Exception("Exception from DB");
            }
        }

        public bool DeleteWaiter(int id)
        {
            bool result;
            try
            {
                result = _managerDataAccess.RemoveWaiter(id);
            }
            catch
            {
                throw new Exception("Exception from DB");
            }

            return result;
        }

        public UserContext AddWaiter(string login, string firstName, string lastName, string password)
        {
            UserContext addingWaiter;
            try
            {
                addingWaiter = _managerDataAccess.AddWaiter(firstName, lastName, login, ClassLib.DataStructures.HashClass.CreateFirstHash(password, login));
            }
            catch (Exception e)
            {
                throw new Exception("Exception from DB");
            }

            return addingWaiter;
        }

        public bool EditWaiter(UserContext waiter, string login, string firstName, string lastName, string password)
        {
            bool result;

            var oldLogin = waiter.Login;
            var oldFirstName = waiter.FirstName;
            var oldSecondName = waiter.LastName;
            //var oldPassword = waiter.Password;

            waiter.Login = login;
            waiter.FirstName = firstName;
            waiter.LastName = lastName;
            //waiter.Password = password;

            try
            {
                result = _managerDataAccess.EditWaiter( waiter);
            }
            catch
            {
                waiter.Login = oldLogin;
                waiter.FirstName = oldFirstName;
                waiter.LastName = oldSecondName;
                //waiter.Password = oldPassword;

                throw new Exception("Exception from DB");
            }

            if (!result)
            {
                waiter.Login = oldLogin;
                waiter.FirstName = oldFirstName;
                waiter.LastName = oldSecondName;
                //waiter.Password = oldPassword;
            }

            return result;
        }
    }
}