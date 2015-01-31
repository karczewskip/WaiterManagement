using BarManager.ManagerDataAccessWCFService;
using System.Collections.Generic;

namespace BarManager.Abstract.Model
{
    public interface IWaiterDataModel
    {
        IList<UserContext> GetAllWaiters();

        bool DeleteWaiter(int id);

        UserContext AddWaiter(string login, string firstName, string lastName, string password);

        bool EditWaiter(UserContext waiter, string login, string firstName, string lastName, string password);
    }
}