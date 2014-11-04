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
using WaiterClient.Abstract;

namespace WaiterClient.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window , IMainWindow
    {
        private IOrderWindow OrderWindow;
        private IMainWindowViewModel MainWindowViewModel;

        public MainWindow(IMainWindowViewModel mainWindowViewModel, IOrderWindow orderWindow)
        {
            MainWindowViewModel = mainWindowViewModel;

            OrderWindow = orderWindow;

            this.DataContext = MainWindowViewModel;

            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string error;
            var result = MainWindowViewModel.LoginUser(LoginTextBox.Text, MyPasswordBox.Password, out error);

            if (result)
            {
                this.Hide();

                OrderWindow.ShowDialog();

                this.Show();
            }
            else
            {
                Messaging.ShowMessage(error);
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }


    }
}
