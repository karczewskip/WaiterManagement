using System;
using System.Linq;
using ClassLib.DataStructures;
using WebUI.ClientDataAccessWebWCFService;
using WebUI.Infrastructure.Abstract;


namespace WebUI.Infrastructure.Concrete
{
    public class DbOrderProcessor : IProcessOrderCommand
    {
        private readonly IProcessOrderCommand _alternateProcessOrderCommand;
        private readonly IClientDataAccess _clientDataAccess;


        public DbOrderProcessor(IProcessOrderCommand alternateProcessOrderCommand, IClientDataAccess clientDataAccess)
        {
            _alternateProcessOrderCommand = alternateProcessOrderCommand;
            _clientDataAccess = clientDataAccess;
        }

        public bool Execute(Cart cart, OrderDetails orderDetails)
        {
            var menuItems = cart.Lines.Select(l => new TupleOfintint(){m_Item1 = l.MenuItem.Id, m_Item2 = l.Quantity}).ToList();

            var order = _clientDataAccess.AddOrder(orderDetails.ClientId, orderDetails.Date, menuItems);

            if (order != null)
                return true;

            // Nie udało się wysłać zamówienie głównym sposobem, próba wysłania drogą alternatywną.
            return _alternateProcessOrderCommand.Execute(cart, orderDetails);
        }
    }
}