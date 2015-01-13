using Ninject;
using WaiterClient.Abstract;
using WaiterClient.Model;
using WaiterClient.View;
using WaiterClient.ViewModel;
using WaiterClient.WaiterDataAccessWCFService;

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
            //container.Bind<IWaiterDataAccessWCFService>().To<WaiterDataAccessWCFServiceClient>().InSingletonScope();
            //container.Bind<IAddOrderWindow>().To<AddOrderWindow>().InSingletonScope();
            //container.Bind<IAddOrderViewModel>().To<AddOrderViewModel>().InSingletonScope();
            container.Bind<IAddItemWindow>().To<AddItemWindow>().InSingletonScope();
            container.Bind<IAddItemViewModel>().To<AddItemViewModel>().InSingletonScope();
            container.Bind<IArchivedOrdersWindow>().To<ArchivedOrdersWindow>().InSingletonScope();
            container.Bind<IArchivedOrdersViewModel>().To<ArchivedOrdersViewModel>().InSingletonScope();
            container.Bind<IShowOrderWindow>().To<ShowOrderWindow>().InSingletonScope();
            container.Bind<IShowOrderViewModel>().To<ShowOrderViewModel>().InSingletonScope();
        }
    }
}
