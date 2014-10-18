using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using HostService.Infrastructure;
using HostService.Abstract;

namespace HostService
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IKernel container;
        private IMainWindow MainWindow;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Infrastructure.Installers.ConfigureContainer(out container);
            ComposeObjects();
            MainWindow.Show();
        }

        private void ComposeObjects()
        {
            MainWindow = this.container.Get<IMainWindow>();
        }
    }
}
