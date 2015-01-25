using Caliburn.Micro;

namespace OrderServiceClient.ViewModels
{
    internal class ConfirmPayViewModel : Screen
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