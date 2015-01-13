using System.Windows;
using WaiterClient.Abstract;
using WaiterClient.WaiterDataAccessWCFService;

namespace WaiterClient.View
{
    /// <summary>
    /// Interaction logic for EditOrderWindow.xaml
    /// </summary>
    public partial class ShowOrderWindow : Window, IShowOrderWindow
    {
        private IShowOrderViewModel ShowOrderViewModel;

        public ShowOrderWindow(IShowOrderViewModel showOrderViewModel)
        {
            ShowOrderViewModel = showOrderViewModel;

            this.DataContext = ShowOrderViewModel;

            InitializeComponent();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        public bool? ShowDialog(Order order)
        {
            ShowOrderViewModel.RefreshOrder(order);

            return ShowDialog();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
