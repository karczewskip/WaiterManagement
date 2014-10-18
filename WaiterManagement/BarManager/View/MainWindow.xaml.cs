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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BarManager.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainWindow
    {
        IMenuManager MenuManager;
        ITableManager TableManager;
        IWaiterManager WaiterManager;

        public MainWindow(IMenuManager menuManager,ITableManager tableManager, IWaiterManager waiterManager)
        {
            MenuManager = menuManager;
            TableManager = tableManager;
            WaiterManager = waiterManager;

            InitializeComponent();
        }

        private void MenuManagerButton_Click(object sender, RoutedEventArgs e)
        {
            MenuManager.Show();
        }

        private void TableManagerButton_Click(object sender, RoutedEventArgs e)
        {
            TableManager.Show();
        }

        private void WaiterManagerButton_Click(object sender, RoutedEventArgs e)
        {
            WaiterManager.Show();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        
    }
}
