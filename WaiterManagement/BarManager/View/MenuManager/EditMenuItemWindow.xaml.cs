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
using ClassLib.DbDataStructures;

namespace BarManager.View
{
    /// <summary>
    /// Interaction logic for EditMenuItemWindow.xaml
    /// </summary>
    public partial class EditMenuItemWindow : Window, IEditMenuItemWindow
    {
        private IEditMenuItemViewModel EditMenuItemViewModel;

        public EditMenuItemWindow(IEditMenuItemViewModel editMenuItemViewModel)
        {
            EditMenuItemViewModel = editMenuItemViewModel;

            this.DataContext = EditMenuItemViewModel;

            InitializeComponent();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        public void ShowDialog(ClassLib.DbDataStructures.MenuItem menuItem)
        {
            EditMenuItemViewModel.RefreshItem(menuItem);

            this.ShowDialog();
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            if (EditMenuItemViewModel.EditMenuItem())
                Close();
        }


    }
}
