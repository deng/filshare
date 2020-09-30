using Autofac;
using Autofac.Extensions.DependencyInjection;
using FilPan.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FilPan.Web
{
    public static class GlobalServiceExtensions
    {
        public static IServiceProvider BuildGlobalServices(this IServiceCollection services)
        {
            var builder = new ContainerBuilder();
            builder.Populate(services);
            var container = builder.Build();

            var serviceProvider = new AutofacServiceProvider(container);
            GlobalServices.Container = container;
            GlobalServices.ServiceProvider = serviceProvider;
            return serviceProvider;
        }
    }
}
