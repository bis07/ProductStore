using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using Ninject;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using ActivityLibrary1.Abstract;
using ActivityLibrary1.Concrete;
using ActivityLibrary1.Entities;
using Moq;
using SportsStore.Infrastructure.Abstract;
using SportsStore.Infrastructure.Concrete;

namespace SportsStore.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;
        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }
        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            kernel.Bind<IProductRepository>().To<EFProductRepository>();

            EmailSettings emailSettings= new EmailSettings
            {
                WriteasFile = bool.Parse(ConfigurationManager.AppSettings["Email.WriteasFile"] ?? "false")
            };
            kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>().WithConstructorArgument("settings", emailSettings);
            kernel.Bind<IAuthProvider>().To<FormsAuthProvider>();
        }

    }
}