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
    /// Interaction logic for ArchivedOrdersWindow.xaml
    /// </summary>
    public partial class ArchivedOrdersWindow : Window, IArchivedOrdersWindow
    {
        private IArchivedOrdersViewModel ArchivedOrdersViewModel;

        public ArchivedOrdersWindow(IArchivedOrdersViewModel archivedOrdersViewModel)
        {
            ArchivedOrdersViewModel = archivedOrdersViewModel;

            this.DataContext = ArchivedOrdersViewModel;

            InitializeComponent();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MoreButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
