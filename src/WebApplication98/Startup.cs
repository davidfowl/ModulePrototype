using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using WebApplication1;

namespace WebApplication98
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var copy = new ServiceCollection();
            foreach (var sd in services)
            {
                copy.Add(sd);
            }

            services.AddSingleton<IServiceCollection>(copy);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IServiceCollection hostingServices, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.Map("/hello", subApp =>
            {
                var moduleEnv = new HostingEnvironment();
                moduleEnv.EnvironmentName = env.EnvironmentName;
                moduleEnv.ApplicationName = "WebApplication1";
                moduleEnv.ContentRootPath = Path.Combine(env.ContentRootPath, @"..", "WebApplication1");
                // TODO: Support this in hosting
                // moduleEnv.PathBase = "/hello";


                // Add the default hosting services every ASP.NET application should have
                var services = new ServiceCollection();
                foreach (var sd in hostingServices)
                {
                    services.Add(sd);
                }

                services.AddSingleton(moduleEnv);
                services.AddSingleton(loggerFactory);
                services.AddHelloServices();

                // Make a child service provider
                subApp.ApplicationServices = services.BuildServiceProvider();

                // Run the module middleware
                subApp.UseHello();
            });

            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
