using Bop.Core.Infrastructure;
using Bop.Web.Framework.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Bop.Web.Framework.Infrastructure
{
    /// <summary>
    /// Represents object for the configuring common features and middleware on application startup
    /// </summary>
    public class BopCommonStartup : IBopStartup
    {
        /// <summary>
        /// Add and configure any of the middleware
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration of the application</param>
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            //compression
            services.AddResponseCompression();

            //add options feature
            services.AddOptions();

            //add distributed memory cache
            services.AddDistributedMemoryCache();

            //add HTTP sesion state feature
            services.AddHttpSession();

            //add default HTTP clients
            services.AddBopHttpClients();

            //add anti-forgery
            services.AddAntiForgery();

            //add localization
            services.AddLocalization();
        }

        /// <summary>
        /// Configure the using of added middleware
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public void Configure(IApplicationBuilder application)
        {
            //use response compression
            application.UseBopResponseCompression();

            //use static files feature
            application.UseBopStaticFiles();

            //use HTTP session
            application.UseSession();

            //use request localization
            application.UseBopRequestLocalization();

            //set request culture
            application.UseCulture();
        }

        /// <summary>
        /// Gets order of this startup configuration implementation
        /// </summary>
        public int Order => 100; //common services should be loaded after error handlers
    }
}