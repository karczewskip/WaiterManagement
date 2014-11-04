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
using BarManager.Abstract;

namespace BarManager.View
{
    /// <summary>
    /// Interaction logic for AddTableManager.xaml
    /// </summary>
    public partial class AddTableWindow : Window, IAddTableWindow
    {
        private IAddTableViewModel AddTableViewModel;
        
        public AddTableWindow(IAddTableViewModel addTableViewmodel)
        {
            AddTableViewModel = addTableViewmodel;

            this.DataContext = AddTableViewModel;

            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string error;
            if (!AddTableViewModel.AddTable(out error))
                Close();
            Messaging.ShowMessage(error);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}
