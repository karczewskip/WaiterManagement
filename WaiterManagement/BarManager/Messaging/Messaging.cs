using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using BarManager.Abstract;
using BarManager.Abstract.ViewModel;

namespace BarManager.Messaging
{
    class Message
    {
        static private IWindowManager _windowManager = IoC.Get<IWindowManager>();
        static private IMessagingViewModel _messaging = IoC.Get<IMessagingViewModel>();

        //static public Message()
        //{
        //    _windowManager = IoC.Get<IWindowManager>();
        //    _messaging = IoC.Get<IMessagingViewModel>();
        //}

        public static void Show(string p)
        {
            _messaging.ErrorMessage = p;
            _windowManager.ShowDialog(_messaging);
        }
    }
}
