using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace LudoApi
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Program
    {
        public static void Main(string[] args)
        {
            Host
                .CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(configure => configure.UseStartup<Startup>())
                .Build()
                .Run();
        }
    }
}