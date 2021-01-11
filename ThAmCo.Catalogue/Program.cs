namespace ThAmCo.Catalogue
{
    using System.Linq;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using ThAmCo.Catalogue.Data;

    public class Program
    {

        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();
            if (args.Contains("-m"))
            {
                using IServiceScope serviceScope = host.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

                DataContext context = serviceScope.ServiceProvider.GetService<DataContext>();
                context.Database.Migrate();
            }
            else
            {
                host.Run();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(configBuilder =>
                {
                    configBuilder.AddJsonFile("secrets/appsettings.secrets.json", optional: true);

                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

    }
}
