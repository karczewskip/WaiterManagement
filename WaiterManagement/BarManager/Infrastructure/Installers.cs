using BarManager.Abstract;
using BarManager.Model;
using BarManager.View;
using BarManager.ViewModel;
using DataAccess;
using Ninject;

namespace BarManager.Infrastructure
{
    public class Installers
    {
        internal static void ConfigureContainer(out IKernel container)
        {
            container = new StandardKernel();

            // MainWindow
            container.Bind<IMainWindow>().To<MainWindow>().InSingletonScope();

            // MenuManager
            container.Bind<IMenuManager>().To<MenuManager>().InSingletonScope();
            container.Bind<IMenuManagerViewModel>().To<MenuManagerViewModel>().InSingletonScope();

            container.Bind<IAddMenuItemWindow>().To<AddMenuItemWindow>().InSingletonScope();
            container.Bind<IAddMenuItemViewModel>().To<AddMenuItemViewModel>().InSingletonScope();

            container.Bind<IAddCategoryItemWindow>().To<AddCategoryItemWindow>().InSingletonScope();
            container.Bind<IAddCategoryViewModel>().To<AddCategoryViewModel>().InSingletonScope();
            
            container.Bind<IEditMenuItemWindow>().To<EditMenuItemWindow>().InSingletonScope();
            container.Bind<IEditMenuItemViewModel>().To<EditMenuItemViewModel>().InSingletonScope();
            
            // TableManager
            container.Bind<ITableManager>().To<TableManager>().InSingletonScope();
            container.Bind<ITableManagerViewModel>().To<TableManagerViewModel>().InSingletonScope();

            container.Bind<IAddTableWindow>().To<AddTableWindow>().InSingletonScope();
            container.Bind<IAddTableViewModel>().To<AddTableViewModel>().InSingletonScope();

            container.Bind<IEditTableWindow>().To<EditTableWindow>().InSingletonScope();
            container.Bind<IEditTableViewModel>().To<EditTableViewModel>().InSingletonScope();
            
            // WaiterManager
            container.Bind<IWaiterManager>().To<WaiterManager>().InSingletonScope();
            container.Bind<IWaiterManagerViewModel>().To<WaiterManagerViewModel>().InSingletonScope();
            
            container.Bind<IAddWaiterWindow>().To<AddWaiterWindow>().InSingletonScope();
            container.Bind<IAddWaiterViewModel>().To<AddWaiterViewModel>().InSingletonScope();

            container.Bind<IEditWaiterWindow>().To<EditWaiterWindow>().InSingletonScope();
            container.Bind<IEditWaiterViewModel>().To<EditWaiterViewModel>().InSingletonScope();
            
            // DataModel
            container.Bind<IBarDataModel>().To<BarDataModel>().InSingletonScope();
            

            // Data access
            container.Bind<IManagerDataAccess>().To<DataAccess.DataAccessClass>().InSingletonScope();
        }
    }
}
