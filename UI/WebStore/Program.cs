using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
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
                   //.ConfigureLogging((host, log) => log
                   //    .ClearProviders()
                   //    .AddDebug()
                   //    .AddConsole(c => c.IncludeScopes = true)
                   //    .AddEventLog()
                   //    .AddFilter("Microsoft", LogLevel.Information)
                   //    .AddFilter<ConsoleLoggerProvider>("Microsoft.EntityFrameworkCore", LogLevel.Warning)
                   // )
                )
               .UseSerilog((host, log) => log.ReadFrom.Configuration(host.Configuration)
                   .MinimumLevel.Debug()
                   .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                   .Enrich.FromLogContext()
                   .WriteTo.Console(
                        outputTemplate: "[{Timestamp:HH:mm:ss.fff} {Level:u3}]{SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}")
                   .WriteTo.RollingFile($@".\Logs\WebStore[{DateTime.Now:yyyy-MM-ddTHH-mm-ss}].log")
                   .WriteTo.File(new JsonFormatter(",", true), $@".\Logs\WebStore[{DateTime.Now:yyyy-MM-ddTHH-mm-ss}].log.json")
                )
            ;
    }
}
