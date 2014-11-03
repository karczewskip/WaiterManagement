using ClassLib.DbDataStructures;
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

        public bool? ShowDialog(ClassLib.DbDataStructures.Order order)
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
