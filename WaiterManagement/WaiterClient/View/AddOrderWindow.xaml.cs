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
    /// Interaction logic for AddOrderWindow.xaml
    /// </summary>
    public partial class AddOrderWindow : Window, IAddOrderWindow
    {
        private IAddOrderViewModel AddOrderViewModel;

        public AddOrderWindow(IAddOrderViewModel addOrderViewModel)
        {
            AddOrderViewModel = addOrderViewModel;

            this.DataContext = AddOrderViewModel;

            InitializeComponent();
        }

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {

        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}
