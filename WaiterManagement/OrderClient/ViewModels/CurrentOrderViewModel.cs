using Caliburn.Micro;
using OrderClient.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderClient.ViewModels
{
    class CurrentOrderViewModel : PropertyChangedBase, IDialogOrder
    {
        private IOrderViewModel _orderWindow;

        public string Salary
        {
            get { return "0.00 PLN to pay"; }
            set
            {
                NotifyOfPropertyChange(() => Salary);
            }
        }

        public CurrentOrderViewModel(IOrderViewModel orderWindow)
        {
            _orderWindow = orderWindow;
        }
    }
}
