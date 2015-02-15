using System;
using System.Web.Mvc;
using System.Web.Routing;
using ClassLib.DataStructures;
using Ninject;
using WebUI.Infrastructure.Abstract;
using WebUI.Infrastructure.Concrete;
using IBaseDataAccess = WebUI.Infrastructure.Abstract.IBaseDataAccess;
using IClientDataAccess = WebUI.Infrastructure.Abstract.IClientDataAccess;
using IManagerDataAccess = WebUI.Infrastructure.Abstract.IManagerDataAccess;

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
            _kernel.Bind<IBaseDataAccess>().To<BaseDataAccess>().InSingletonScope();
            _kernel.Bind<IClientDataAccess>().To<ClientDataAccess>().InSingletonScope();
            _kernel.Bind<IManagerDataAccess>().To<ManagerDataAccess>().InSingletonScope();

            _kernel.Bind<IProcessOrderCommand>().To<DbOrderProcessor>();
            _kernel.Bind<IProcessOrderCommand>().To<EmailOrderProcessor>().WhenInjectedInto<DbOrderProcessor>();

            _kernel.Bind<IAuthProvider>().To<FormsAuthProvider>().InSingletonScope();
        }
    }
}