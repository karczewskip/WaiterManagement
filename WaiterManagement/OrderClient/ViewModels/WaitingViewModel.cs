using Caliburn.Micro;
using OrderClient.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderClient.ViewModels
{
    class WaitingViewModel : PropertyChangedBase, IWaitingViewModel
    {
        private readonly IOrderDataModel _orderDataModel;

        public WaitingViewModel( IOrderDataModel orderDataModel)
        {
            _orderDataModel = orderDataModel;
        }

        public string Message
        {
            get { return _orderDataModel.GetCurrentOrderMessage(); }
        }

        public void RefreshMessage()
        {
            NotifyOfPropertyChange(() => Message);
        }
    }
}
