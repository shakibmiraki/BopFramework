using System;
using System.Linq;
using System.Text;
using Bop.Core;
using Bop.Core.Configuration;
using Bop.Data;
using Bop.Core.Http;
using Bop.Core.Infrastructure;
using Bop.Services.Authentication;
using Bop.Services.Installation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Profiling.Storage;
using Bop.Services.Logging;
using Bop.Core.Domain;
using Bop.Services.Security;
using Newtonsoft.Json.Serialization;
using System.Net;
using Bop.Core.Security;
using Microsoft.AspNetCore.DataProtection;
using Bop.Core.Redis;

namespace Bop.Web.Framework.Infrastructure.Extensions
{
    /// <summary>
    /// Represents extensions of IServiceCollection
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add services to the application and configure service provider
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration of the application</param>
        /// <param name="hostingEnvironment">Hosting environment</param>
        /// <returns>Configured service provider</returns>
        public static IServiceProvider ConfigureApplicationServices(this IServiceCollection services,
            IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {

            //most of API providers require TLS 1.2 nowadays
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            //add BopConfig configuration parameters
            var bopConfig = services.ConfigureStartupConfig<BopConfig>(configuration.GetSection("Bop"));

            //add spa configuration parameters
            services.ConfigureStartupConfig<SpaConfig>(configuration.GetSection("Spa"));

            //add spa configuration parameters
            services.ConfigureStartupConfig<JwtConfig>(configuration.GetSection("Jwt"));

            //add hosting configuration parameters
            services.ConfigureStartupConfig<HostingConfig>(configuration.GetSection("Hosting"));

            //add accessor to HttpContext
            services.AddHttpContextAccessor();


            //validate host environment
            //CommonHelper.ValidateHostEnvironment(webHostEnvironment);

            //create default file provider
            CommonHelper.DefaultFileProvider = new BopFileProvider(webHostEnvironment);

            //initialize plugins
            services.AddMvcCore();

            //create engine and configure service provider
            var engine = EngineContext.Create();
            var serviceProvider = engine.ConfigureServices(services, configuration, bopConfig);

            //further actions are performed only when the database is installed
            if (!DataSettingsManager.DatabaseIsInstalled)
                CreateAndSeedDatabase();

            return serviceProvider;
        }

        private static void CreateAndSeedDatabase()
        {
            var dataProvider = EngineContext.Current.Resolve<IBopDataProvider>();
            var dataSetting = DataSettingsManager.LoadSettings(reloadSettings: true);

            dataProvider.CreateDatabase(dataSetting.Collation);
            dataProvider.InitializeDatabase();
            var installationService = EngineContext.Current.Resolve<IInstallationService>();
            installationService.InstallRequiredData(dataSetting.Phone, dataSetting.Password);

            dataSetting.IsDatabaseInstalled = true;
            DataSettingsManager.SaveSettings(dataSetting);
        }

        /// <summary>
        /// Create, bind and register as service the specified configuration parameters 
        /// </summary>
        /// <typeparam name="TConfig">Configuration parameters</typeparam>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Set of key/value application configuration properties</param>
        /// <returns>Instance of configuration parameters</returns>
        public static TConfig ConfigureStartupConfig<TConfig>(this IServiceCollection services, IConfiguration configuration) where TConfig : class, new()
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            //create instance of config
            var config = new TConfig();

            //bind it to the appropriate section of configuration
            configuration.Bind(config);

            //and register it as a service
            services.AddSingleton(config);

            return config;
        }

        /// <summary>
        /// Register HttpContextAccessor
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddHttpContextAccessor(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        /// <summary>
        /// Adds services required for anti-forgery support
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddAntiForgery(this IServiceCollection services)
        {
            //override cookie name
            services.AddAntiforgery(options =>
            {
                options.Cookie.Name = $"{BopCookieDefaults.Prefix}{BopCookieDefaults.AntiforgeryCookie}";

                //whether to allow the use of anti-forgery cookies from SSL protected page on the other store pages which are not
                options.Cookie.SecurePolicy = CookieSecurePolicy.None;
            });
        }

        /// <summary>
        /// Adds services required for application session state
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddHttpSession(this IServiceCollection services)
        {
            services.AddSession(options =>
            {
                options.Cookie.Name = $"{BopCookieDefaults.Prefix}{BopCookieDefaults.SessionCookie}";
                options.Cookie.HttpOnly = true;

                //whether to allow the use of session values from SSL protected page on the other store pages which are not
                options.Cookie.SecurePolicy = CookieSecurePolicy.None;
            });
        }


        /// <summary>
        /// Adds data protection services
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddNopDataProtection(this IServiceCollection services)
        {
            //check whether to persist data protection in Redis
            var bopConfig = services.BuildServiceProvider().GetRequiredService<BopConfig>();
            if (bopConfig.RedisEnabled && bopConfig.UseRedisToStoreDataProtectionKeys)
            {
                //store keys in Redis
                services.AddDataProtection().PersistKeysToStackExchangeRedis(() =>
                {
                    var redisConnectionWrapper = EngineContext.Current.Resolve<IRedisConnectionWrapper>();
                    return redisConnectionWrapper.GetDatabase(bopConfig.RedisDatabaseId ?? (int)RedisDatabaseNumber.DataProtectionKeys);
                }, BopDataProtectionDefaults.RedisDataProtectionKey);
            }
            else
            {
                var dataProtectionKeysPath = CommonHelper.DefaultFileProvider.MapPath(BopDataProtectionDefaults.DataProtectionKeysPath);
                var dataProtectionKeysFolder = new System.IO.DirectoryInfo(dataProtectionKeysPath);

                //configure the data protection system to persist keys to the specified directory
                services.AddDataProtection().PersistKeysToFileSystem(dataProtectionKeysFolder);
            }
        }


        /// <summary>
        /// Adds authentication service
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddBopAuthentication(this IServiceCollection services)
        {

            var jwtConfig = services.BuildServiceProvider().GetRequiredService<JwtConfig>();

            // Needed for jwt auth.
            services
                .AddAuthentication(options =>
                {
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = jwtConfig.Issuer, // site that makes the token
                        ValidateIssuer = false, // TODO: change this to avoid forwarding attacks
                        ValidAudience = jwtConfig.Audience, // site that consumes the token
                        ValidateAudience = false, // TODO: change this to avoid forwarding attacks
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Key)),
                        ValidateIssuerSigningKey = true, // verify signature to avoid tampering
                        ValidateLifetime = true, // validate the expiration
                        ClockSkew = TimeSpan.Zero // tolerance for the expiration date
                    };
                    cfg.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger>();
                            logger.Error("Authentication failed.", context.Exception);
                            return System.Threading.Tasks.Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            var tokenValidatorService = context.HttpContext.RequestServices.GetRequiredService<ITokenValidatorService>();
                            return tokenValidatorService.ValidateAsync(context);
                        },
                        OnMessageReceived = context =>
                         {
                             return System.Threading.Tasks.Task.CompletedTask;
                         },
                        OnChallenge = context =>
                        {
                            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger>();
                            logger.Error("OnChallenge error", context.AuthenticateFailure);
                            return System.Threading.Tasks.Task.CompletedTask;
                        }
                    };
                });

        }

        /// <summary>
        /// Add and configure MVC for the application
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <returns>A builder for configuring MVC services</returns>
        public static IMvcBuilder AddBopMvc(this IServiceCollection services)
        {

            //add basic MVC feature
            var mvcBuilder = services.AddControllersWithViews();

            mvcBuilder.AddRazorRuntimeCompilation();

            services.AddRazorPages();

            //MVC now serializes JSON with camel case names by default, use this code to avoid it
            mvcBuilder.AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());


            //add fluent validation
            mvcBuilder.AddFluentValidation(configuration =>
            {
                //register all available validators from Nop assemblies
                var assemblies = mvcBuilder.PartManager.ApplicationParts
                    .OfType<AssemblyPart>()
                    .Where(part => part.Name.StartsWith("Bop", StringComparison.InvariantCultureIgnoreCase))
                    .Select(part => part.Assembly);
                configuration.RegisterValidatorsFromAssemblies(assemblies);

                //implicit/automatic validation of child properties
                configuration.ImplicitlyValidateChildProperties = true;
            });

            //register controllers as services, it'll allow to override them
            mvcBuilder.AddControllersAsServices();

            return mvcBuilder;
        }


        /// <summary>
        /// Add CORS Policy for the application
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <returns>A builder for configuring MVC services</returns>
        public static IServiceCollection AddBopCors(this IServiceCollection services)
        {

            var spaConfig = services.BuildServiceProvider().GetRequiredService<SpaConfig>();
            var cors = services.AddCors(options =>
            {
                options.AddPolicy(BopInfrastructureDefaults.BopCorsPolicyName,
                    builder => builder
                        .WithOrigins(spaConfig.CorsPath) //Note:  The URL must be specified without a trailing slash (/).
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            return cors;
        }


        /// <summary>
        /// Add and configure MiniProfiler service
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddBopMiniProfiler(this IServiceCollection services)
        {
            //whether database is already installed
            if (!DataSettingsManager.DatabaseIsInstalled)
                return;

            services.AddMiniProfiler(miniProfilerOptions =>
            {
                //use memory cache provider for storing each result
                ((MemoryCacheStorage)miniProfilerOptions.Storage).CacheDuration = TimeSpan.FromMinutes(60);

                //whether MiniProfiler should be displayed
                miniProfilerOptions.ShouldProfile = request =>
                    EngineContext.Current.Resolve<HostedSiteInformationSettings>().DisplayMiniProfilerInPublicStore;

                //determine who can access the MiniProfiler results
                miniProfilerOptions.ResultsAuthorize = request =>
                    !EngineContext.Current.Resolve<HostedSiteInformationSettings>().DisplayMiniProfilerForAdminOnly ||
                    EngineContext.Current.Resolve<IPermissionService>().Authorize(StandardPermissionProvider.AccessAdminPanel);
            });
        }

        /// <summary>
        /// Add and configure default HTTP clients
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddBopHttpClients(this IServiceCollection services)
        {
            //default client
            services.AddHttpClient();
        }
    }
}