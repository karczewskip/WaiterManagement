namespace OrderServiceClient.Abstract
{
    public interface IConfirmDialogFactory
    {

        IConfirmDialogViewModel GetConfirmDialog(WaiterDataAccessWCFService.Order order);

        IConfirmDialogViewModel GetConfirmDialog(string p);
    }
}