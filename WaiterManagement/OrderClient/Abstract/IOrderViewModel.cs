using OrderClient.ClientDataAccessWCFService;

namespace OrderClient.Abstract
{
    public interface IOrderViewModel
    {
        void CloseAddItemDialog();

        void CheckIfIsPosibleToAddOrder();

        void SetOrderState(OrderState state);

        void ShowPayingWindow();

        void CloseOrder();

        void NotyfyOrderOnHold();

        void SetMainWindowReference(IMainWindowViewModel _mainWindowViewModel);
    }
}
