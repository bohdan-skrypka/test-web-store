using Ninject;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Domain.Abstract;
using Domain.App_Data;
using System.Configuration;

namespace WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;
        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        private void AddBindings()
        {
            kernel.Bind<IGoodsRepository>().To<EFGoodsRepository>();

            EmailSettings emailSet = new EmailSettings
            {
                WriteToFile = bool.Parse(ConfigurationManager.AppSettings["Email.WriteToFile"] ?? "false")
            };
            kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>()
                .WithConstructorArgument("settings",emailSet);
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
    }
}