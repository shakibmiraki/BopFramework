using System.Globalization;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Bop.Core;
using Bop.Core.Configuration;
using Bop.Data;
using Bop.Core.Domain.Common;
using Bop.Core.Infrastructure;
using Bop.Services.Localization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Hosting;
using Bop.Services.Logging;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Net.Http.Headers;
using Bop.Web.Framework.Globalization;

namespace Bop.Web.Framework.Infrastructure.Extensions
{
    /// <summary>
    /// Represents extensions of IApplicationBuilder
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Configure the application HTTP request pipeline
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public static void ConfigureRequestPipeline(this IApplicationBuilder application)
        {
            EngineContext.Current.ConfigureRequestPipeline(application);
        }


        public static void StartEngine(this IApplicationBuilder application)
        {
            var engine = EngineContext.Current;

            //further actions are performed only when the database is installed
            if (DataSettingsManager.DatabaseIsInstalled)
            {
                //initialize and start schedule tasks
                Services.Tasks.TaskManager.Instance.Initialize();
                Services.Tasks.TaskManager.Instance.Start();

                //log application start
                engine.Resolve<ILogger>().Information("Application started");
            }
        }


        /// <summary>
        /// Add exception handling
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public static void UseBopExceptionHandler(this IApplicationBuilder application)
        {
            var bopConfig = EngineContext.Current.Resolve<BopConfig>();
            var webHostEnvironment = EngineContext.Current.Resolve<IWebHostEnvironment>();
            var useDetailedExceptionPage = bopConfig.DisplayFullErrorStack || webHostEnvironment.IsDevelopment();
            if (useDetailedExceptionPage)
            {
                //get detailed exceptions for developing and testing purposes
                application.UseDeveloperExceptionPage();
            }
            else
            {
                //or use special exception handler
                application.UseExceptionHandler("/Error/Error");
            }

            //log errors
            application.UseExceptionHandler(handler =>
            {
                handler.Run(context =>
                {
                    var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                    if (exception == null)
                        return Task.CompletedTask;

                    try
                    {
                        //check whether database is installed
                        if (DataSettingsManager.DatabaseIsInstalled)
                        {

                            //get current customer
                            var currentCustomer = EngineContext.Current.Resolve<IWorkContext>().CurrentCustomer;

                            //log error
                            EngineContext.Current.Resolve<ILogger>().Error(exception.Message, exception, currentCustomer);
                        }
                    }
                    finally
                    {
                        //rethrow the exception to show the error page
                        ExceptionDispatchInfo.Throw(exception);
                    }

                    return Task.CompletedTask;
                });
            });
        }


        /// <summary>
        /// Adds a special handler that checks for responses with the 400 status code (bad request)
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public static void UseBadRequestResult(this IApplicationBuilder application)
        {
            application.UseStatusCodePages(context =>
            {
                //handle 404 (Bad request)
                if (context.HttpContext.Response.StatusCode == StatusCodes.Status400BadRequest)
                {
                    var logger = EngineContext.Current.Resolve<ILogger>();
                    var workContext = EngineContext.Current.Resolve<IWorkContext>();
                    logger.Error("Error 400. Bad request", null, customer: workContext.CurrentCustomer);
                }

                return Task.CompletedTask;
            });
        }

        /// <summary>
        /// Configure middleware for dynamically compressing HTTP responses
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public static void UseBopResponseCompression(this IApplicationBuilder application)
        {
            //whether to use compression (gzip by default)
            if (DataSettingsManager.DatabaseIsInstalled && EngineContext.Current.Resolve<CommonSettings>().UseResponseCompression)
                application.UseResponseCompression();
        }


        /// <summary>
        /// Configure static file serving
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public static void UseBopStaticFiles(this IApplicationBuilder application)
        {
            static void staticFileResponse(StaticFileResponseContext context)
            {
                if (!DataSettingsManager.DatabaseIsInstalled)
                    return;

                var commonSettings = EngineContext.Current.Resolve<CommonSettings>();
                if (!string.IsNullOrEmpty(commonSettings.StaticFilesCacheControl))
                    context.Context.Response.Headers.Append(HeaderNames.CacheControl, commonSettings.StaticFilesCacheControl);
            }

            //common static files
            application.UseStaticFiles(new StaticFileOptions { OnPrepareResponse = staticFileResponse });


        }


        /// <summary>
        /// Adds the authentication middleware, which enables authentication capabilities.
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public static void UseBopAuthentication(this IApplicationBuilder application)
        {
            //check whether database is installed
            if (!DataSettingsManager.DatabaseIsInstalled)
                return;

            application.UseAuthentication();
            //application.UseMiddleware<AuthenticationMiddleware>();
        }

        /// <summary>
        /// Configure the request localization feature
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public static void UseBopRequestLocalization(this IApplicationBuilder application)
        {
            application.UseRequestLocalization(options =>
            {
                if (!DataSettingsManager.DatabaseIsInstalled)
                    return;

                //prepare supported cultures
                var cultures = EngineContext.Current.Resolve<ILanguageService>().GetAllLanguages()
                    .OrderBy(language => language.DisplayOrder)
                    .Select(language => new CultureInfo(language.LanguageCulture)).ToList();
                options.SupportedCultures = cultures;
                options.DefaultRequestCulture = new RequestCulture(cultures.LastOrDefault());
            });
        }


        /// <summary>
        /// Set current culture info
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public static void UseCulture(this IApplicationBuilder application)
        {
            //check whether database is installed
            if (!DataSettingsManager.DatabaseIsInstalled)
                return;

            application.UseMiddleware<CultureMiddleware>();
        }


        /// <summary>
        /// Configure Cors
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public static void UseBopCors(this IApplicationBuilder application)
        {
            application.UseCors(BopInfrastructureDefaults.BopCorsPolicyName);
        }


        /// <summary>
        /// Configure Endpoints routing
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public static void UseBopEndpoints(this IApplicationBuilder application)
        {
            //Add the EndpointRoutingMiddleware
            application.UseRouting();

            //Execute the endpoint selected by the routing middleware
            application.UseEndpoints(endpoints =>
            {
                //register all routes
                endpoints.MapRazorPages();
                //EngineContext.Current.Resolve<IRoutePublisher>().RegisterRoutes(endpoints);
            });
        }

    }
}
