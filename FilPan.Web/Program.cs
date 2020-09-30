using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FilPan.BackgroundServices;
using FilPan.Services;

namespace filshareapp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureAppConfiguration(configBuilder =>
            {
                AppSettingsService.AppendConfigurations(configBuilder, "appsettings.json");
            }).ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseUrls("http://*:14100").UseStartup<Startup>();
            }).ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<Worker>();
            });
    }
}
