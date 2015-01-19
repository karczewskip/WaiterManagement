using OrderClient.ClientDataAccessWCFService;

namespace OrderClient.Abstract
{
    interface IOrderViewModel
    {
        void CloseAddItemDialog();

        void CheckIfIsPosibleToAddOrder();

        void SetOrderState(OrderState state);
    }
}
