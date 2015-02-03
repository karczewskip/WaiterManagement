using System;
using System.Web.Mvc;
using System.Web.Routing;
using ClassLib.DataStructures;
using DataAccess;
using Ninject;
using WebUI.Infrastructure.Abstract;
using WebUI.Infrastructure.Concrete;

namespace WebUI.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private readonly IKernel _kernel;

        public NinjectControllerFactory()
        {
            _kernel = new StandardKernel();
            AddBindings();
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null
            ? null
            : (IController)_kernel.Get(controllerType);
        }

        private void AddBindings()
        {
            _kernel.Bind<IBaseDataAccess>().To<DataAccessClass>().InSingletonScope();
            _kernel.Bind<IClientDataAccess>().To<DataAccessClass>().InSingletonScope();
            _kernel.Bind<IManagerDataAccess>().To<DataAccessClass>().InSingletonScope();

            _kernel.Bind<IProcessOrderCommand>().To<DbOrderProcessor>();
            _kernel.Bind<IProcessOrderCommand>().To<EmailOrderProcessor>().WhenInjectedInto<DbOrderProcessor>();

            _kernel.Bind<IAuthProvider>().To<FormsAuthProvider>().InSingletonScope();
        }
    }
}