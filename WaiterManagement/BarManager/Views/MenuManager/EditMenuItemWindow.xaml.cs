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

namespace BarManager.Views
{
    /// <summary>
    /// Interaction logic for EditMenuItemWindow.xaml
    /// </summary>
    public partial class EditMenuItemWindow : Window, IEditMenuItemWindow
    {

        public EditMenuItemWindow()
        {
            InitializeComponent();
        }
    }
}
