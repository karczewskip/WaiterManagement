using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarManager.Abstract;

namespace BarManager.ViewModels
{
    class MessagingViewModel : IMessagingViewModel
    {
        public string ErrorMessage { get; set; }
    }
}
