using BarManager.Abstract;
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

namespace BarManager.View
{
    /// <summary>
    /// Interaction logic for AddWaiterWindow.xaml
    /// </summary>
    public partial class AddWaiterWindow : Window, IAddWaiterWindow
    {
        private IAddWaiterViewModel AddWaiterViewModel;

        public AddWaiterWindow(IAddWaiterViewModel addWaiterViewModel)
        {
            AddWaiterViewModel = addWaiterViewModel;

            this.DataContext = AddWaiterViewModel;

            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string error;
            if (AddWaiterViewModel.AddWaiter(out error))
                Close();
            else
                MessageBox.Show(error);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}
