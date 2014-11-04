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
    /// Interaction logic for EditTableWindow.xaml
    /// </summary>
    public partial class EditTableWindow : Window, IEditTableWindow
    {
        private IEditTableViewModel EditTableViewModel;

        public EditTableWindow(IEditTableViewModel editTableViewModel)
        {
            EditTableViewModel = editTableViewModel;

            this.DataContext = EditTableViewModel;

            InitializeComponent();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        public void ShowDialog(ClassLib.DbDataStructures.Table table)
        {
            EditTableViewModel.RefreshItem(table);

            this.ShowDialog();
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            string error;
            if (EditTableViewModel.EditTable(out error))
                Close();
            else
                Messaging.ShowMessage(error);
        }
    }
}
