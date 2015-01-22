using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderServiceClient.ViewModels
{
    class ConfirmPayViewModel : Screen
    {



        public void Confirm()
        {
            TryClose(true);
        }

        public void Reject()
        {
            TryClose(false);
        }
    }
}
