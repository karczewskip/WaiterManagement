using BarManager.Abstract;
using System.Windows;

namespace BarManager.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindowView : Window, IMainWindow
    {
        public MainWindowView()
        {
            InitializeComponent();
        }
    }
}
