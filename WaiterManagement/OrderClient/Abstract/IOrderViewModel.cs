using OrderClient.ClientDataAccessWCFService;

namespace OrderClient.Abstract
{
    interface IOrderViewModel
    {
        void CloseAddItemDialog();

        void CheckIfIsPosibleToAddOrder();

        void SetOrderState(OrderState state);

        void ShowPayingWindow();

        void CloseOrder();

        void NotyfyOrderOnHold();
    }
}
