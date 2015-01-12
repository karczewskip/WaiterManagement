using OrderClient.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderClient.ViewModels
{
    class WaitingViewModel : IWaitingViewModel
    {
        private IOrderViewModel _orderViewModel;
        private IOrderDataModel _orderDataModel;

        public WaitingViewModel(IOrderViewModel orderViewModel, IOrderDataModel orderDataModel)
        {
            _orderViewModel = orderViewModel;
            _orderDataModel = orderDataModel;
        }

        public string Message
        {
            get { return _orderDataModel.GetCurrentOrderMessage(); }
        }
    }
}
