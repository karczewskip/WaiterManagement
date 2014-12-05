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
        private IKernel _container;
        private IMainWindow _mainWindow;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(MyHandler);

            Infrastructure.Installers.ConfigureContainer(out _container);
            ComposeObjects();
            _mainWindow.Show();
        }

        private void MyHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Messaging.ShowMessage("Unexpected problem was emerged: \n" + ((Exception) e.ExceptionObject).Message);
            Application.Current.Shutdown();
        }

        private void ComposeObjects()
        {
            _mainWindow = this._container.Get<IMainWindow>();
        }
    }
}
