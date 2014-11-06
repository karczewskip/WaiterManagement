using BarManager.Abstract;
using BarManager.View;
using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BarManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IKernel Container;
        private IMainWindow MainWindow;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(MyHandler);

            Infrastructure.Installers.ConfigureContainer(out Container);
            ComposeObjects();
            MainWindow.Show();
        }

        private void MyHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Messaging.ShowMessage("Unexpected problem was emerged: \n" + ((Exception) e.ExceptionObject).Message);
            Application.Current.Shutdown();
        }

        private void ComposeObjects()
        {
            MainWindow = this.Container.Get<IMainWindow>();
        }
    }
}
