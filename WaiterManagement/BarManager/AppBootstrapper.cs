using BarManager.Abstract;
using BarManager.ManagerDataAccessWCFService;
using BarManager.Model;
using BarManager.ViewModels;
using Caliburn.Micro;
using Ninject;
using System;
using System.Collections.Generic;
using System.Windows;

namespace BarManager
{
    public class AppBootstrapper : BootstrapperBase
    {
        private IKernel _kernel;

        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<MainWindowViewModel>();
        }

        protected override void Configure()
        {
            _kernel = new StandardKernel();

            _kernel.Bind<IWindowManager>().To<WindowManager>().InSingletonScope();

            //MenuManager
            _kernel.Bind<IMenuManagerViewModel>().To<MenuManagerViewModel>().InSingletonScope();
            _kernel.Bind<IAddMenuItemViewModel>().To<AddMenuItemViewModel>().InSingletonScope();
            _kernel.Bind<IAddCategoryViewModel>().To<AddCategoryViewModel>().InSingletonScope();
            _kernel.Bind<IEditMenuItemViewModel>().To<EditMenuItemViewModel>().InSingletonScope();

          
            //WaiterManager
            _kernel.Bind<IWaiterManagerViewModel>().To<WaiterManagerViewModel>().InSingletonScope();
            _kernel.Bind<IAddWaiterViewModel>().To<AddWaiterViewModel>().InSingletonScope();
            _kernel.Bind<IEditWaiterViewModel>().To<EditWaiterViewModel>().InSingletonScope();

            //TableManager
            _kernel.Bind<ITableManagerViewModel>().To<TableManagerViewModel>().InSingletonScope();
            _kernel.Bind<IAddTableViewModel>().To<AddTableViewModel>().InSingletonScope();
            _kernel.Bind<IEditTableViewModel>().To<EditTableViewModel>().InSingletonScope();

            //DataModel
            _kernel.Bind<IBarDataModel>().To<BarDataModel>().InSingletonScope();
            _kernel.Bind<IManagerDataAccessWCFService>().To<ManagerDataAccessWCFServiceClient>().InSingletonScope();
            _kernel.Bind<IMessagingViewModel>().To<MessagingViewModel>().InSingletonScope();
        }

        protected override void OnExit(object sender, EventArgs e)
        {
            _kernel.Dispose();
            base.OnExit(sender, e);
        }

        protected override object GetInstance(Type service, string key)
        {
            if (service == null)
                throw new ArgumentNullException("service");

            return _kernel.Get(service);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _kernel.GetAll(service);
        }

        protected override void BuildUp(object instance)
        {
            _kernel.Inject(instance);
        }
    }
}