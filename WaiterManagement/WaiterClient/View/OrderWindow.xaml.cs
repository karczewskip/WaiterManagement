using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WaiterClient.Abstract;

namespace WaiterClient.View
{
    /// <summary>
    /// Interaction logic for OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window, IOrderWindow
    {
        private IOrderWindowViewModel OrderWindowViewModel;
        private IAddOrderWindow AddOrderWindow;
        private IShowOrderWindow ShowOrderWindow;
        private IArchivedOrdersWindow ArchivedOrderWindow;

        public OrderWindow(IOrderWindowViewModel orderWindowViewModel, IAddOrderWindow addOrderWindow, IShowOrderWindow showOrderWindow, IArchivedOrdersWindow archivedOrderWindow)
        {
            OrderWindowViewModel = orderWindowViewModel;

            AddOrderWindow = addOrderWindow;
            ShowOrderWindow = showOrderWindow;
            ArchivedOrderWindow = archivedOrderWindow;

            this.DataContext = OrderWindowViewModel;

            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddOrderWindow.ShowDialog();
        }

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
                ShowOrderWindow.ShowDialog(OrdersListView.SelectedItem as ClassLib.DbDataStructures.Order);
        }

        private void OrdersListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (OrdersListView.SelectedItem != null)
                ShowOrderWindow.ShowDialog(OrdersListView.SelectedItem as ClassLib.DbDataStructures.Order);
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
            if (!OrderWindowViewModel.RelizeOrder(out error))
                Messaging.ShowMessage(error);
        }

        
    }
}
