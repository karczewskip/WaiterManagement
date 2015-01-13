using System.Windows;
using System.Windows.Input;
using WaiterClient.Abstract;
using WaiterClient.WaiterDataAccessWCFService;

namespace WaiterClient.View
{
    /// <summary>
    /// Interaction logic for OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window, IOrderWindow
    {
        private IOrderWindowViewModel OrderWindowViewModel;
        //private IAddOrderWindow AddOrderWindow;
        private IShowOrderWindow ShowOrderWindow;
        private IArchivedOrdersWindow ArchivedOrderWindow;

        public OrderWindow(IOrderWindowViewModel orderWindowViewModel/*, IAddOrderWindow addOrderWindow*/, IShowOrderWindow showOrderWindow, IArchivedOrdersWindow archivedOrderWindow)
        {
            OrderWindowViewModel = orderWindowViewModel;

            //AddOrderWindow = addOrderWindow;
            ShowOrderWindow = showOrderWindow;
            ArchivedOrderWindow = archivedOrderWindow;

            this.DataContext = OrderWindowViewModel;

            InitializeComponent();
        }

        //private void AddButton_Click(object sender, RoutedEventArgs e)
        //{
        //    AddOrderWindow.ShowDialog();
        //}

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ArchiveButton_Click(object sender, RoutedEventArgs e)
        {
            ArchivedOrderWindow.ShowDialog();
        }

        private void ShowButton_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersListView.SelectedItem != null)
                ShowOrderWindow.ShowDialog(OrdersListView.SelectedItem as Order);
        }

        private void OrdersListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (OrdersListView.SelectedItem != null)
                ShowOrderWindow.ShowDialog(OrdersListView.SelectedItem as Order);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            OrderWindowViewModel.LogOut();

            e.Cancel = true;
            this.Hide();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            string error;
            if (!OrderWindowViewModel.CancelOrder(out error))
                Messaging.ShowMessage(error);
        }

        private void RelizeButton_Click(object sender, RoutedEventArgs e)
        {
            string error;
            if (!OrderWindowViewModel.RealizeOrder(out error))
                Messaging.ShowMessage(error);
        }

        
    }
}
