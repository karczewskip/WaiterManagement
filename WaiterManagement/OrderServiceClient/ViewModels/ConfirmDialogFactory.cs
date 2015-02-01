using OrderServiceClient.Abstract;
using OrderServiceClient.WaiterDataAccessWCFService;

namespace OrderServiceClient.ViewModels
{
    public class ConfirmDialogFactory : IConfirmDialogFactory
    {

        public IConfirmDialogViewModel GetConfirmDialog(Order order)
        {
            return new ConfirmDialogViewModel(order);
        }

        public IConfirmDialogViewModel GetConfirmDialog(string message)
        {
            return new ConfirmDialogViewModel(message);
        }
    }
}