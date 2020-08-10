using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Bop.Core.Domain.Localization;
using Bop.Data;
using Bop.Services.Localization;
using Bop.Web.Framework.Mvc.Routing;
using Bop.Web.Framework;

namespace Bop.Web.Infrastructure
{
    /// <summary>
    /// Represents provider that provided basic routes
    /// </summary>
    public partial class RouteProvider : IRouteProvider
    {
        #region Methods

        /// <summary>
        /// Register routes
        /// </summary>
        /// <param name="endpointRouteBuilder">Route builder</param>
        public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
        {
            var pattern = string.Empty;

            //areas
            endpointRouteBuilder.MapControllerRoute(name: "areaRoute",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            //login
            endpointRouteBuilder.MapControllerRoute("AuthLogin", $"{pattern}/api/auth/login/",
                new { controller = "Customer", action = "Login" });

            //logout
            endpointRouteBuilder.MapControllerRoute("AuthLogout", $"{pattern}/api/auth/logout/",
                new { controller = "Customer", action = "Logout" });

            //register
            endpointRouteBuilder.MapControllerRoute("AuthRegister", $"{pattern}/api/auth/register/",
                new { controller = "Customer", action = "Register" });

            //activate
            endpointRouteBuilder.MapControllerRoute("AuthActivate", $"{pattern}/api/auth/activate/",
                new { controller = "Customer", action = "Activate" });

            //resend activation code
            endpointRouteBuilder.MapControllerRoute("AuthResend", $"{pattern}/api/auth/resend/",
                new { controller = "Customer", action = "Resend" });

            //get profile
            endpointRouteBuilder.MapControllerRoute("CustomerGetResend", $"{pattern}/api/customer/getprofile/",
                new { area = AreaNames.Admin, controller = "Customer", action = "GetProfile" });

            //set profile
            endpointRouteBuilder.MapControllerRoute("CustomerUpdateProfile", $"{pattern}/api/customer/updateprofile/",
                new { area = AreaNames.Admin, controller = "Customer", action = "UpdateProfile" });


            ////admin customer
            //endpointRouteBuilder.MapControllerRoute("", "admin/customer/authorize",
            //    new { controller = "Customer", action = "Authorize", area = AreaNames.Admin });

            //endpointRouteBuilder.MapControllerRoute("", "admin/customer/refreshtoken",
            //    new { controller = "Customer", action = "RefreshToken", area = AreaNames.Admin });

            ////admin localization
            //endpointRouteBuilder.MapControllerRoute("", "admin/localization/export",
            //    new { controller = "Localization", action = "ExportLanguage", area = AreaNames.Admin });

            //endpointRouteBuilder.MapControllerRoute("", "admin/localization/import",
            //    new { controller = "Localization", action = "ImportLanguage", area = AreaNames.Admin });

            //endpointRouteBuilder.MapControllerRoute("", "admin/localization/languages",
            //    new { controller = "Localization", action = "GetAllLanguages", area = AreaNames.Admin });
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a priority of route provider
        /// </summary>
        public int Priority => 0;

        #endregion
    }
}
