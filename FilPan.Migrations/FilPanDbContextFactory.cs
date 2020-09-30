using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using FilPan.Services;
using FilPan.Web;
using FilPan.Entities;

namespace FilPan.Migrations
{
    public class FilPanDbContextFactory : IDesignTimeDbContextFactory<FilPanDbContext>
    {
        public FilPanDbContext CreateDbContext(string[] args)
        {
            var config = AppSettingsService.BuildConfiguration("appsettings.json");
            var services = new ServiceCollection();
            services.AddAppServices(config);
            var serviceProvider = services.BuildGlobalServices();
            return serviceProvider.GetService<FilPanDbContext>();
        }
    }
}
