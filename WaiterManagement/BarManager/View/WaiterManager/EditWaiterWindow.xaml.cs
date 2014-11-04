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
    /// Interaction logic for EditWaiterWindow.xaml
    /// </summary>
    public partial class EditWaiterWindow : Window, IEditWaiterWindow
    {
        private IEditWaiterViewModel EditWaiterViewModel;
 
        public EditWaiterWindow(IEditWaiterViewModel editWaiterViewModel)
        {
            EditWaiterViewModel = editWaiterViewModel;

            this.DataContext = EditWaiterViewModel;

            InitializeComponent();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        public void ShowDialog(ClassLib.DbDataStructures.WaiterContext Waiter)
        {
            EditWaiterViewModel.RefreshItem(Waiter);

            this.ShowDialog();
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            string error;
            if (EditWaiterViewModel.EditWaiter(out error))
                Close();
            else
                Messaging.ShowMessage(error);

        }


    }
}
