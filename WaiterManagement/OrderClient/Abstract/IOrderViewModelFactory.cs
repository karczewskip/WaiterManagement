namespace OrderClient.Abstract
{
    public interface IOrderViewModelFactory
    {
        void SetMainWindowReference(IMainWindowViewModel mainWindowViewModel);

        IOrderViewModel GetOrderViewModel();
    }
}