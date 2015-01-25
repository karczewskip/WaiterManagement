using System.Threading;
using System.Windows;
using System.Windows.Threading;
using OrderServiceClient.Abstract;
using OrderServiceClient.WaiterDataAccessWCFService;

namespace OrderServiceClient.ViewModels
{
    public class ConfirmDialogViewModel : IConfirmDialogViewModel
    {
        private Order _order;

        private bool IsClicked { get; set; }
        private bool Result { get; set; }

        public ConfirmDialogViewModel(Order order)
        {
            // TODO: Complete member initialization
            this._order = order;
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