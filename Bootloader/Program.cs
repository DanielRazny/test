using Bootloader.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using UpdateService.Services.VersionChecker;
using static System.Net.Mime.MediaTypeNames;

namespace Bootloader
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection, configuration);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var bootloader = serviceProvider.GetRequiredService<BootLoader>();

            await bootloader.RunAsync().ConfigureAwait(false);

            Console.ReadLine();
        }

        private static void ConfigureServices(ServiceCollection serviceCollection, IConfiguration configuration)
        {
            // AppSettings IOptions configuration
            serviceCollection.Configure<ApplicationOptions>(options =>
            {
                options.UpdateServerUrl = "https://localhost:7057";
                options.ApplicationPath = "C:\\temp\\Test\\SampleApplication.exe";
            });

            serviceCollection.AddTransient<BootLoader>();
            serviceCollection.AddTransient<IVersionCompareHandler, VersionCompareHandler>();
            serviceCollection.AddHttpClient();
        }
    }
}
