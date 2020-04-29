using Bop.Core.Infrastructure;
using Bop.Web.Framework.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bop.Web.Framework.Infrastructure
{
    /// <summary>
    /// Represents object for the configuring MVC on application startup
    /// </summary>
    public class BopMvcStartup : IBopStartup
    {
        /// <summary>
        /// Add and configure any of the middleware
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration of the application</param>
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {

            //add MiniProfiler services
            services.AddBopMiniProfiler();

            //add cors policy
            services.AddBopCors();

            //add and configure MVC feature
            services.AddBopMvc();

        }

        /// <summary>
        /// Configure the using of added middleware
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public void Configure(IApplicationBuilder application)
        {
            //use MiniProfiler
            application.UseMiniProfiler();

            //Endpoints routing
            application.UseBopEndpointsWithCors();
        }

        /// <summary>
        /// Gets order of this startup configuration implementation
        /// </summary>
        public int Order => 1000; //MVC should be loaded last
    }
}