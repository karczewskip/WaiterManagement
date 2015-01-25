namespace OrderClient.Abstract
{
    public interface IMainWindowViewModel
    {
        void CancelOrder();
        void LogIn();
        void StartGettingOrders();
        void CloseOrder();
    }
}