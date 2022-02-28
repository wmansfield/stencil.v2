using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Placeholder.Primary.Foundation;
using Unity;
using Unity.Microsoft.DependencyInjection;
using Zero.Foundation;
using Zero.Foundation.Web;

namespace Placeholder.Website
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            FoundationHost foundationHost = new FoundationHost();
            do
            {
                foundationHost.Host = Host.CreateDefaultBuilder(args)
                    .UseUnityServiceProvider(foundationHost.Container)
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>((context) => new Startup(context.Configuration, context.HostingEnvironment, foundationHost.Container));
                    })
                    .Build();
                    
                await foundationHost.Host.RunAsync(foundationHost.CancellationSource.Token);
            }
            while (foundationHost.ShouldRestart);
        }
    }
}
