using System;
using System.Collections.Generic;
using ClassLib.DataStructures;
using WebUI.ClientDataAccessWebWCFService;

namespace WebUI.Infrastructure.Abstract
{
    public interface IClientDataAccess : IBaseDataAccess
    {
        UserContext AddClient(string firstName, string lastName, string login, string password);
        Order AddOrder(int userId, DateTime orderTime, IEnumerable<TupleOfintint> menuItems);
        IEnumerable<Order> GetOrders(int clientId);
    }
}
