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
    /// Interaction logic for WaiterManager.xaml
    /// </summary>
    public partial class WaiterManager : Window, IWaiterManager
    {
        private IWaiterManagerViewModel WaiterManagerViewModel;
        private IAddWaiterWindow AddWaiterWindow;
        private IEditWaiterWindow EditWaiterWindow;

        public WaiterManager(IWaiterManagerViewModel waiterManagerViewModel, IAddWaiterWindow addWaiterWindow, IEditWaiterWindow editWaiterWindow)
        {
            WaiterManagerViewModel = waiterManagerViewModel;
            AddWaiterWindow = addWaiterWindow;
            EditWaiterWindow = editWaiterWindow;

            this.DataContext = WaiterManagerViewModel;

            InitializeComponent();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            string error;
            if (WaiterManagerViewModel.DeleteSelectedItem(out error))
                MessageBox.Show(error);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddWaiterWindow.ShowDialog();
        }

        private void WaitersListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (WaitersListView.SelectedItem != null)
                EditWaiterWindow.ShowDialog(WaitersListView.SelectedItem as ClassLib.DbDataStructures.WaiterContext);

            WaitersListView.Items.Refresh();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
