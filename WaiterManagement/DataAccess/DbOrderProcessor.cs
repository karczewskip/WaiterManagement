using ClassLib.DataStructures;

namespace DataAccess
{
    public class DbOrderProcessor : IProcessOrderCommand
    {
        private readonly IProcessOrderCommand _processOrderCommand;

        public DbOrderProcessor(IProcessOrderCommand processOrderCommand)
        {
            _processOrderCommand = processOrderCommand;
        }

        public bool Execute(Cart cart, OrderDetails orderDetails)
        {
            //TODO: Implementing adding order to DB and some logic here
            var resultAddingToDB = true;

            if (!resultAddingToDB)
                return false;

            var resultAnotherCommand = _processOrderCommand.Execute(cart, orderDetails);

            if (!resultAnotherCommand)
            {
                //TODO: For some reasons it failed, Reverse the changes
                return false;
            }

            return true;
        }
    }
}