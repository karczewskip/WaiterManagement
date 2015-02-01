using OrderClient.ViewModels;

namespace OrderClient.Abstract
{
    internal interface IPayingWindow
    {
        void SetOrderWindowReference(OrderViewModel orderViewModel);
    }
}