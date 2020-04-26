using Bop.Core;
using Bop.Services.Authentication;
using Bop.Services.Events;
using Bop.Services.Localization;
using Bop.Services.Security;
using Bop.Web.Areas.Admin.Models;
using Bop.Web.Framework;
using Bop.Web.Framework.Controllers;
using Bop.Web.Framework.UI;
using Bop.Web.Models.Customers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Bop.Web.Areas.Admin.Controllers
{
    [Area(AreaNames.Admin)]
    public class CustomerController : AdminBaseController
    {

        private readonly IPermissionService _permissionService;
        private readonly ILocalizationService _localizationService;
        private readonly ITokenStoreService _tokenStoreService;
        private readonly ITokenFactoryService _tokenFactoryService;
        private readonly IAntiForgeryCookieService _antiForgeryCookieService;

        public CustomerController(IPermissionService permissionService, 
            ILocalizationService localizationService, 
            ITokenStoreService tokenStoreService,
            ITokenFactoryService tokenFactoryService, 
            IAntiForgeryCookieService antiForgeryCookieService, 
            IEventPublisher eventPublisher, 
            IWorkContext workContext)
        {
            _permissionService = permissionService;
            _localizationService = localizationService;
            _tokenStoreService = tokenStoreService;
            _tokenFactoryService = tokenFactoryService;
            _antiForgeryCookieService = antiForgeryCookieService;
        }

        /// <summary>
        /// we add no message because it is not UI base operation 
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Authorize([FromBody]PermissionRequest permission)
        {
            var response = new PermissionResponse { Result = ResultType.Error };
            var permissionRecord = _permissionService.GetPermissionRecordBySystemName(permission.PermissionSystemName);
            if (permissionRecord is null)
            {
                return Ok(response);
            }

            var isAuthorize = _permissionService.Authorize(permissionRecord);
            if (!isAuthorize)
            {
                return Ok(response);
            }

            response.Result = ResultType.Success;
            return Ok(response);
        }


        
//        public bool IsAuthenticated()
//        {
//            return User.Identity.IsAuthenticated;
//        }


        public IActionResult RefreshToken([FromBody]JToken jsonBody)
        {

            var response = new LoginResponse { Result = ResultType.Error };

            var refreshTokenValue = jsonBody.Value<string>("refreshToken");
            if (string.IsNullOrWhiteSpace(refreshTokenValue))
            {
                response.Messages.Add(_localizationService.GetResource("account.token.refreshtoken.nullrefreshtoken"));
                return BadRequest(response);
            }

            var token = _tokenStoreService.FindToken(refreshTokenValue);
            if (token == null)
            {
                response.Messages.Add(_localizationService.GetResource("account.token.refreshtoken.nulltoken"));
                return Unauthorized(response);
            }


            var jwtToken = _tokenFactoryService.CreateJwtTokens(token.Customer);
            _tokenStoreService.AddCustomerToken(token.Customer, jwtToken.RefreshTokenSerial, jwtToken.AccessToken, _tokenFactoryService.GetRefreshTokenSerial(refreshTokenValue));
            _antiForgeryCookieService.RegenerateAntiForgeryCookies(jwtToken.Claims);

            response.Result = ResultType.Success;
            response.AccessToken = jwtToken.AccessToken;
            response.RefreshToken = jwtToken.RefreshToken;

            return Ok(response);
        }

    }
}