using BarManager.Abstract;
using BarManager.Concrete;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarManager.Infrastructure
{
    class Installers
    {
        internal static void ConfigureContainer(out IKernel container)
        {
            container = new StandardKernel();
            container.Bind<IMainWindow>().To<MainWindow>().InSingletonScope();
        }
    }
}
