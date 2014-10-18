using BarManager.Abstract;
using BarManager.View;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarManager.Infrastructure
{
    public class Installers
    {
        internal static void ConfigureContainer(out IKernel container)
        {
            container = new StandardKernel();
            container.Bind<IMainWindow>().To<MainWindow>().InSingletonScope();
            container.Bind<IMenuManager>().To<MenuManager>().InSingletonScope();
            container.Bind<ITableManager>().To<TableManager>().InSingletonScope();
            container.Bind<IWaiterManager>().To<WaiterManager>().InSingletonScope();
        }
    }
}
