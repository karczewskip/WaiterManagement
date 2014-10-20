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
    /// Interaction logic for AddCategoryItemWindow.xaml
    /// </summary>
    public partial class AddCategoryItemWindow : Window, IAddCategoryItemWindow
    {
        private IAddCategoryViewModel AddCategoryViewModel;

        public AddCategoryItemWindow(IAddCategoryViewModel addCategoryViewModel)
        {
            AddCategoryViewModel = addCategoryViewModel;

            this.DataContext = AddCategoryViewModel;

            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (AddCategoryViewModel.AddCategory())
                Close();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}
