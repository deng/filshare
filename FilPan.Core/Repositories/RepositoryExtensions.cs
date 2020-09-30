using System;
using FilPan.Entities;
using FilPan.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FilPan.Repositories
{
    public static class RepositoryExtensions
    {
        public static IServiceCollection UseRepositories(this IServiceCollection services)
        {
            services.UseDbContext();
            services.AddScoped<IFilPanRepository, FilPanRepository>();
            return services;
        }

        public static IServiceCollection UseDbContext(this IServiceCollection services)
        {
            services.AddDbContext<FilPanDbContext>((serviceProvider, options) =>
            {
                var setting = serviceProvider.GetRequiredService<FilePanDbSetting>();
                if (setting == null)
                    throw new ArgumentNullException(nameof(setting));

                var connectionName = typeof(FilPanDbContext).Name;
                UseDbServerType(options, connectionName, setting);
            });
            return services;
        }

        private static void UseDbServerType(DbContextOptionsBuilder options, string connectionName, IConnSetting connSetting)
        {
            var connectionString = connSetting.Connection;
            var migrationsAssemblyName = connSetting.MigrationsAssemblyName;
            switch (connSetting.DbServer)
            {
                case DbServerType.Sqlite:
                    options.UseSqlite(connectionString, q =>
                    {
                        if (!string.IsNullOrEmpty(migrationsAssemblyName))
                        {
                            q.MigrationsAssembly(migrationsAssemblyName);
                        }
                    });
                    break;
                case DbServerType.InMemory:
                    options.UseInMemoryDatabase(databaseName: connectionName);
                    break;
            }
        }
    }
}
