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

        public OrderWindow(IOrderWindowViewModel orderWindowViewModel, IAddOrderWindow addOrderWindow)
        {
            OrderWindowViewModel = orderWindowViewModel;

            AddOrderWindow = addOrderWindow;

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

        }

        private void ShowButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OrdersListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            OrderWindowViewModel.LogOut();

            e.Cancel = true;
            this.Hide();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
