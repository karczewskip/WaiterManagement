using BarManager.Abstract;
using BarManager.View;
using BarManager.ViewModel;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarManager.Model;
using DataAccess;

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
            container.Bind<IAddMenuItemWindow>().To<AddMenuItemWindow>().InSingletonScope();
            container.Bind<IAddCategoryItemWindow>().To<AddCategoryItemWindow>().InSingletonScope();
            container.Bind<IBarDataModel>().To<BarDataModel>().InSingletonScope();
            container.Bind<IAddCategoryViewModel>().To<AddCategoryViewModel>().InSingletonScope();
            container.Bind<IAddMenuItemViewModel>().To<AddMenuItemViewModel>().InSingletonScope();
            

            container.Bind<IMenuManagerViewModel>().To<MenuManagerViewModel>().InSingletonScope();


            // Data access
            container.Bind<IManagerDataAccess>().To<DataAccess.DataAccess>().InSingletonScope();
        }
    }
}
