using OrderClient.ClientDataAccessWCFService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderClient.Abstract
{
    interface IOrderViewModel
    {
        

        void CloseAddItemDialog();

        void CheckIfIsPosibleToAddOrder();

        void SetOrderState(OrderState state);
    }
}
