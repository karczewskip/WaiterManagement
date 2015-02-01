using Caliburn.Micro;
using OrderClient.Abstract;

namespace OrderClient.ViewModels
{
    public class OrderViewModelFactory : IOrderViewModelFactory
    {
        private IMainWindowViewModel _mainWindowViewModel;

        public void SetMainWindowReference(IMainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
        }

        public IOrderViewModel GetOrderViewModel()
        {
            var orderViewModel = IoC.Get<IOrderViewModel>();
            orderViewModel.SetMainWindowReference(_mainWindowViewModel);
            return orderViewModel;
        }
    }
}