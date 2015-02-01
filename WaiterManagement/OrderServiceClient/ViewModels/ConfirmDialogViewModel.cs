using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using OrderServiceClient.Abstract;
using OrderServiceClient.WaiterDataAccessWCFService;

namespace OrderServiceClient.ViewModels
{
    public class ConfirmDialogViewModel : IConfirmDialogViewModel
    {
        public string Message { get; set; }

        private bool IsClicked { get; set; }
        private bool Result { get; set; }

        public ConfirmDialogViewModel(Order order)
        {
            Message = SetMessageToConfirmOrder(order);
        }

        public ConfirmDialogViewModel(string message)
        {
            Message = message;
        }

        private static string SetMessageToConfirmOrder(Order order)
        {
            var content = new StringBuilder();
            foreach(var o in order.MenuItems)
            {
                content.Append(o.MenuItem.Name + "(" + o.Quantity + "),");
            }
            return "Client:  " + order.Client.Login + 
                    ",\nTable: " + order.Table.Description + 
                    ",\nContent: " + content.ToString(); 
        }

        public bool GetResult()
        {
            Result = false;
            IsClicked = false;

            while (!IsClicked)
            {
                if (Application.Current.Dispatcher.HasShutdownStarted ||
                Application.Current.Dispatcher.HasShutdownFinished)
                {
                    break;
                }

                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate { }));
                Thread.Sleep(20);
            }

            return Result;
        }

        public void Accept()
        {
            Result = true;
            IsClicked = true;
        }

        public void Reject()
        {
            Result = false;
            IsClicked = true;
        }
    }
}