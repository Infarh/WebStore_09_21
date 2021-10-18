using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebStore.Services.Data;

namespace WebStore
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host_builder = CreateHostBuilder(args);
            var host = host_builder.Build();

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(host => host
                   .UseStartup<Startup>()
                )
            ;
    }
}
