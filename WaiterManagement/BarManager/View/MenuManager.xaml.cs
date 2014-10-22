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
    /// Interaction logic for MenuManager.xaml
    /// </summary>
    public partial class MenuManager : Window, IMenuManager
    {
        IMenuManagerViewModel MenuManagerViewModel;
        IAddMenuItemWindow AddMenuItemWindow;
        IEditMenuItemWindow EditMenuItemWindow;
        IAddCategoryItemWindow AddCategoryItemWindow;
        

        public MenuManager(IMenuManagerViewModel menuManagerViewModel, IAddMenuItemWindow addMenuItemView, IEditMenuItemWindow editMenuItemWindow, IAddCategoryItemWindow addCategoryItemWindow)
        {
            MenuManagerViewModel = menuManagerViewModel;
            AddMenuItemWindow = addMenuItemView;
            EditMenuItemWindow = editMenuItemWindow;
            AddCategoryItemWindow = addCategoryItemWindow;

            this.DataContext = menuManagerViewModel;

            InitializeComponent();
        }

        /// <summary>
        /// Close button reaction
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            MenuManagerViewModel.DeleteSelectedItem();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddMenuItemWindow.ShowDialog();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void AddCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            AddCategoryItemWindow.ShowDialog();
        }

        private void MenuItemsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (MenuItemsListView.SelectedItem != null)
                EditMenuItemWindow.ShowDialog(MenuItemsListView.SelectedItem as ClassLib.DbDataStructures.MenuItem);
        }

        

        
    }
}
