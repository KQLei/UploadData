using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace UploadMyData
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                    .UseUrls("http://127.0.0.1:8083")
                    .UseKestrel(options => options.Limits.MaxRequestBodySize = 200 * 1024 * 1024);
                });
    }
}
