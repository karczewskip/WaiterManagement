using BarManager.Views;
using System;
using System.Windows;

namespace BarManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(MyHandler);
        }

        private void MyHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Messaging.ShowMessage("Unexpected problem was emerged: \n" + ((Exception)e.ExceptionObject).Message);
            Application.Current.Shutdown();
        }
    }
}
