using System.Windows;
using WaiterClient.Abstract;
using WaiterClient.WaiterDataAccessWCFService;

namespace WaiterClient.View
{
    /// <summary>
    /// Interaction logic for AddItemWindow.xaml
    /// </summary>
    public partial class AddItemWindow : Window, IAddItemWindow
    {
        IAddItemViewModel AddItemViewModel;

        public AddItemWindow(IAddItemViewModel addItemViewModel)
        {
            AddItemViewModel = addItemViewModel;

            this.DataContext = AddItemViewModel;

            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(((Button)sender).Tag.GetType().ToString());
            AddItemViewModel.AddItem(((System.Windows.Controls.Button)sender).Tag as MenuItem);
        }
    }
}
