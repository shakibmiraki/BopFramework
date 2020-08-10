using Bop.Core;
using Bop.Core.Domain.Customers;
using Bop.Services.Authentication;
using Bop.Services.Common;
using Bop.Services.Localization;
using Bop.Services.Security;
using Bop.Web.Areas.Admin.Models;
using Bop.Web.Framework;
using Bop.Web.Framework.Controllers;
using Bop.Web.Framework.UI;
using Bop.Web.Models.Customers;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Newtonsoft.Json.Linq;
using System;

namespace Bop.Web.Areas.Admin.Controllers
{
    [Area(AreaNames.Admin)]
    public class CustomerController : AdminBaseController
    {

        private readonly IPermissionService _permissionService;
        private readonly ILocalizationService _localizationService;
        private readonly ITokenStoreService _tokenStoreService;
        private readonly ITokenFactoryService _tokenFactoryService;
        private readonly IWorkContext _workContext;
        private readonly IGenericAttributeService _genericAttributeService;

        public CustomerController(IPermissionService permissionService,
            ILocalizationService localizationService,
            ITokenStoreService tokenStoreService,
            ITokenFactoryService tokenFactoryService,
            IWorkContext workContext,
            IGenericAttributeService genericAttributeService)
        {
            _permissionService = permissionService;
            _localizationService = localizationService;
            _tokenStoreService = tokenStoreService;
            _tokenFactoryService = tokenFactoryService;
            _workContext = workContext;
            _genericAttributeService = genericAttributeService;
        }

        /// <summary>
        /// we add no message because it is not UI base operation 
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Authorize([FromBody] PermissionRequest permission)
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

        public IActionResult RefreshToken([FromBody] JToken jsonBody)
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

            response.Result = ResultType.Success;
            response.AccessToken = jwtToken.AccessToken;
            response.RefreshToken = jwtToken.RefreshToken;

            return Ok(response);
        }


        /// <summary>
        /// Get User profile
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetProfile()
        {
            var response = new ProfileResponse { Result = ResultType.Error };
            var customer = _workContext.CurrentCustomer;
            if (customer == null)
            {
                response.Messages.Add(_localizationService.GetResource("Account.AccountActivation.customerNotExist"));
                return BadRequest(response);
            }

            var genericAttributes = _genericAttributeService.GetAttributesForEntity(customer.Id, customer.GetType().Name);
            response.FirstName = genericAttributes.SingleOrDefault(a => a.Key == BopCustomerDefaults.FirstName)?.Value;
            response.LastName = genericAttributes.SingleOrDefault(a => a.Key == BopCustomerDefaults.LastName)?.Value;
            response.Email = genericAttributes.SingleOrDefault(a => a.Key == BopCustomerDefaults.Email)?.Value;
            response.NationalCode = genericAttributes.SingleOrDefault(a => a.Key == BopCustomerDefaults.NationalCode)?.Value;
            //response.BirthDate = DateTimeOffset.Parse(genericAttributes.SingleOrDefault(a => a.Key == BopCustomerDefaults.BirthDate)?.Value);
            //response.Gender = bool.Parse(genericAttributes.SingleOrDefault(a => a.Key == BopCustomerDefaults.Gender)?.Value);
            response.Result = ResultType.Success;
            return Ok(response);
        }

        /// <summary>
        /// Get User profile
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UpdateProfile([FromBody] ProfileRequest profile)
        {
            var response = new ProfileResponse { Result = ResultType.Error };
            var customer = _workContext.CurrentCustomer;

            _genericAttributeService.SaveAttribute(customer, BopCustomerDefaults.FirstName, profile.FirstName);
            _genericAttributeService.SaveAttribute(customer, BopCustomerDefaults.LastName, profile.LastName);
            _genericAttributeService.SaveAttribute(customer, BopCustomerDefaults.Email, profile.Email);
            _genericAttributeService.SaveAttribute(customer, BopCustomerDefaults.NationalCode, profile.NationalCode);
            _genericAttributeService.SaveAttribute(customer, BopCustomerDefaults.BirthDate, profile.BirthDate);
            _genericAttributeService.SaveAttribute(customer, BopCustomerDefaults.Gender, profile.Gender);

            response.Result = ResultType.Success;
            return Ok(response);
        }

    }
}