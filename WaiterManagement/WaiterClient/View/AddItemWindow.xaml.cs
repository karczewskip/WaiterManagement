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
using ClassLib.DbDataStructures;

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
            AddItemViewModel.AddItem(((Button)sender).Tag as ClassLib.DbDataStructures.MenuItem);
        }
    }
}
