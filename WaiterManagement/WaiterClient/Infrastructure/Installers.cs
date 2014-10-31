using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaiterClient.Abstract;
using WaiterClient.Model;
using WaiterClient.View;
using WaiterClient.ViewModel;
using DataAccess;

namespace WaiterClient.Infrastructure
{
    class Installers
    {
        internal static void ConfigureContainer(out IKernel container)
        {
            container = new StandardKernel();
            container.Bind<IMainWindow>().To<MainWindow>().InSingletonScope();
            container.Bind<IOrderWindow>().To<OrderWindow>().InSingletonScope();
            container.Bind<IMainWindowViewModel>().To<MainWindowViewModel>().InSingletonScope();
            container.Bind<IOrderWindowViewModel>().To<OrderWindowViewModel>().InSingletonScope();
            container.Bind<IWaiterClientModel>().To<WaiterClientModel>().InSingletonScope();
            container.Bind<IWaiterDataAccess>().To<DataAccessClass>().InSingletonScope();
            container.Bind<IAddOrderWindow>().To<AddOrderWindow>().InSingletonScope();
            container.Bind<IAddOrderViewModel>().To<AddOrderViewModel>().InSingletonScope();
            container.Bind<IAddItemWindow>().To<AddItemWindow>().InSingletonScope();
            container.Bind<IAddItemViewModel>().To<AddItemViewModel>().InSingletonScope();
        }
    }
}
