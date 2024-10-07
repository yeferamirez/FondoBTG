using Autofac.Extensions.DependencyInjection;
using PensionFund;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        var pathToContentRoot = AppDomain.CurrentDomain.BaseDirectory;
        var configurationRoot = new ConfigurationBuilder()
            .SetBasePath(pathToContentRoot)
            .AddJsonFile("appsettings.json", true)
            .Build();

        return Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureServices(services => { services.AddAutofac(); })
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.SetBasePath(pathToContentRoot);
                config.AddJsonFile("appsettings.json", true);
                config.AddConfiguration(configurationRoot);
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder
                    .UseKestrel()
                .UseIISIntegration()
                    .UseStartup<Startup>();
            });
    }
}
