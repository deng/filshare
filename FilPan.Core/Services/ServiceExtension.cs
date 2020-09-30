using FilPan.Managers;
using FilPan.Repositories;
using FilPan.Sdk;
using FilPan.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FilPan.Services
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.BindConfiguration(configuration);
            services.AddSingleton<LotusClient>();
            services.UseRepositories();
            services.AddSingleton<IPanPasswordHasher, PanPasswordHasher>();
            services.AddSingleton<FilPanManager>();
            return services;
        }
    }
}
