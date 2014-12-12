
namespace BarManager.Views
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
