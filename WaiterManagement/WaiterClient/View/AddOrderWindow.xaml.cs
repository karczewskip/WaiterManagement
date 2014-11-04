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
        private IAddItemWindow AddItemWindow;

        public AddOrderWindow(IAddOrderViewModel addOrderViewModel, IAddItemWindow addItemWindow)
        {
            AddOrderViewModel = addOrderViewModel;
            AddItemWindow = addItemWindow;

            this.DataContext = AddOrderViewModel;

            InitializeComponent();

            AddOrderViewModel.AddObserverListView(ItemsListView);
        }

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            AddItemWindow.ShowDialog();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            string error;
            if ( !AddOrderViewModel.DeleteSelectedItem(out error))
                MessageBox.Show(error);
        }

        private void AddOrderButton_Click(object sender, RoutedEventArgs e)
        {
            string error;
            if (!AddOrderViewModel.AddOrder(out error))
                Messaging.ShowMessage(error);
            else
                Close();
        }
    }
}
