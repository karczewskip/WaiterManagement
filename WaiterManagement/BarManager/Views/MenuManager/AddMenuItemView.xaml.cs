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
using BarManager.Abstract.ViewModel;

namespace BarManager.Views
{
    /// <summary>
    /// Interaction logic for AddMenuItemWindow.xaml
    /// </summary>
    public partial class AddMenuItemView
    {
        public AddMenuItemView(IAddMenuItemViewModel addMenuItemViewModel)
        {
            InitializeComponent();
        }
    }
}
