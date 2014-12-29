using Caliburn.Micro;
using OrderClient.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderClient.ViewModels
{
    class AddItemViewModel : PropertyChangedBase, IDialogOrder
    {
        private IOrderViewModel _orderWindow;

        public AddItemViewModel(IOrderViewModel orderWindow)
        {
            _orderWindow = orderWindow;
        }

        public void Exit()
        {
            _orderWindow.CloseAddItemDialog();
        }
    }
}
