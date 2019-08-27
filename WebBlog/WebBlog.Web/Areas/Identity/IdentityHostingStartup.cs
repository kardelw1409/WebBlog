using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(WebBlog.Web.Areas.Identity.IdentityHostingStartup))]
namespace WebBlog.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}