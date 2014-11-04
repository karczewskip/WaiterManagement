using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarManager.View
{
    static public class Messaging
    {
        static MessageWindow MW;

        static public void ShowMessage(string message)
        {
            if(MW == null)
                MW = new MessageWindow();

            MW.ShowMessage(message);
        }
    }
}
