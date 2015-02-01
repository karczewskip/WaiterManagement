using Caliburn.Micro;
using Ninject;
using OrderClient.Abstract;
using OrderClient.Model;
using OrderClient.Service_Communication;
using OrderClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OrderClient
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
            _kernel.Bind<IOrderNotyficator>().To<OrderNotyficator>().InSingletonScope();
            _kernel.Bind<IOrderDataModel>().To<OrderDataModel>().InSingletonScope();
            _kernel.Bind<IClientDataAccess>().To<ClientDataAccess>().InSingletonScope();

            //Access View Model
            _kernel.Bind<IAccessViewModel>().To<AccessViewModel>().InSingletonScope();
            _kernel.Bind<IDialogRegister>().To<RegisterViewModel>().InSingletonScope();
            _kernel.Bind<IDialogLogin>().To<LoginViewModel>().InSingletonScope();

            _kernel.Bind<IChooseTabelViewModel>().To<ChooseTabelViewModel>().InSingletonScope();

            //Order View Model
            _kernel.Bind<IOrderViewModelFactory>().To<OrderViewModelFactory>().InSingletonScope();
            _kernel.Bind<IOrderViewModel>().To<OrderViewModel>().InTransientScope();
            _kernel.Bind<ICurrentOrder>().To<CurrentOrderViewModel>().InTransientScope();
            _kernel.Bind<IDialogAddingItem>().To<AddItemViewModel>().InTransientScope();
            _kernel.Bind<IWaitingViewModel>().To<WaitingViewModel>().InTransientScope();
            _kernel.Bind<IPayingWindow>().To<PayingViewModel>().InTransientScope();

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
