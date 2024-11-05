using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace InterviewTakeawayTask2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }


        ///Middleware being called in startup.cs. Looks like it just setting up swagger, enabling/mapping routing and controllers, and setting up a singleton service.
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
