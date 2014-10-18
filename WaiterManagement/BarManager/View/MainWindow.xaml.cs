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

        public MainWindow(IMenuManager menuManager)
        {
            MenuManager = menuManager;

            InitializeComponent();
        }

        private void MenuManagerButton_Click(object sender, RoutedEventArgs e)
        {
            MenuManager.Show();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            //MenuManager.Close();

            base.OnClosing(e);
        }

        
    }
}
