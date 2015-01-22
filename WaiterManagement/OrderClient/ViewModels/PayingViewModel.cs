using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderClient.Abstract;

namespace OrderClient.ViewModels
{
    class PayingViewModel : IPayingWindow
    {
        private IOrderDataModel _orderDataModel;
        private IOrderViewModel _orderViewModel;

        public PayingViewModel(IOrderViewModel orderViewModel, IOrderDataModel orderDataModel)
        {
            _orderViewModel = orderViewModel;
            _orderDataModel = orderDataModel;
        }

        public void Pay()
        {
            _orderDataModel.Pay();
            _orderViewModel.CloseOrder();
        }
    }
}
