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
    /// Interaction logic for TableManager.xaml
    /// </summary>
    public partial class TableManager : Window, ITableManager
    {
        ITableManagerViewModel TableManagerViewModel;
        IAddTableWindow AddTableWindow;
        IEditTableWindow EditTableWindow;

        public TableManager(ITableManagerViewModel tableManagerViewModel, IAddTableWindow addTableWindow, IEditTableWindow editTableWindow)
        {
            TableManagerViewModel = tableManagerViewModel;
            AddTableWindow = addTableWindow;
            EditTableWindow = editTableWindow;

            this.DataContext = TableManagerViewModel;

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
            if (!TableManagerViewModel.DeleteSelectedItem(out error))
                Messaging.ShowMessage(error);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddTableWindow.ShowDialog();
        }

        private void TablesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (TablesListView.SelectedItem != null)
                EditTableWindow.ShowDialog(TablesListView.SelectedItem as ClassLib.DbDataStructures.Table);

            TablesListView.Items.Refresh();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        
    }
}
