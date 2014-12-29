using Caliburn.Micro;
using Ninject;
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
