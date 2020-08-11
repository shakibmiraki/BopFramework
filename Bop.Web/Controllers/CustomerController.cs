using System;
using System.Linq;
using System.Security.Claims;
using Bop.Core;
using Bop.Services.Authentication;
using Bop.Services.Common;
using Bop.Services.Events;
using Bop.Services.Localization;
using Bop.Services.Messages;
using Bop.Services.Customers;
using Bop.Web.Framework.Controllers;
using Bop.Web.Framework.UI;
using Microsoft.AspNetCore.Mvc;
using Bop.Core.Domain.Customers;
using Bop.Web.Models.Customers;

namespace Bop.Web.Controllers
{
    public class CustomerController : BaseController
    {
        private readonly ICustomerService _customerService;
        private readonly ICustomerRegistrationService _customerRegistrationService;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly ITokenStoreService _tokenStoreService;
        private readonly ITokenFactoryService _tokenFactoryService;
        private readonly CustomerSettings _customerSettings;
        private readonly IAuthenticationService _authenticationService;

        public CustomerController(ICustomerService customerService,
            ICustomerRegistrationService customerRegistrationService,
            IEventPublisher eventPublisher,
            ILocalizationService localizationService,
            IWorkContext workContext,
            IGenericAttributeService genericAttributeService,
            IWorkflowMessageService workflowMessageService,
            ITokenStoreService tokenStoreService,
            ITokenFactoryService tokenFactoryService,
            CustomerSettings customerSettings,
            IAuthenticationService authenticationService)
        {
            _customerService = customerService;
            _customerRegistrationService = customerRegistrationService;
            _eventPublisher = eventPublisher;
            _localizationService = localizationService;
            _workContext = workContext;
            _genericAttributeService = genericAttributeService;
            _workflowMessageService = workflowMessageService;
            _tokenStoreService = tokenStoreService;
            _tokenFactoryService = tokenFactoryService;
            _customerSettings = customerSettings;
            _authenticationService = authenticationService;
        }


        #region Login / Logout

        [HttpPost]
        public IActionResult Login([FromBody] LoginRequest model)
        {
            var response = new LoginResponse { Result = ResultType.Error };

            if (ModelState.IsValid)
            {
                model.Mobile = model.Mobile;
                var loginResult = _customerRegistrationService.ValidateCustomer(model.Mobile, model.Password);

                switch (loginResult)
                {
                    case CustomerLoginResults.Successful:
                        {
                            var customer = _customerService.GetCustomerByPhone(model.Mobile);

                            //sign in new customer
                            var token = _tokenFactoryService.CreateJwtTokens(customer);
                            _tokenStoreService.AddCustomerToken(customer, token.RefreshTokenSerial, token.AccessToken, null);

                            //raise event       
                            _eventPublisher.Publish(new CustomerLoggedinEvent(customer));

                            response.Result = ResultType.Success;
                            response.AccessToken = token.AccessToken;
                            response.RefreshToken = token.RefreshToken;

                            return Ok(response);
                        }
                    case CustomerLoginResults.CustomerNotExist:
                        response.Messages.Add(
                            _localizationService.GetResource("account.login.wrongcredentials.customernotexist"));
                        break;
                    case CustomerLoginResults.Deleted:
                        response.Messages.Add(
                            _localizationService.GetResource("account.login.wrongcredentials.deleted"));
                        break;
                    case CustomerLoginResults.NotActive:
                        response.Messages.Add(
                            _localizationService.GetResource("account.login.wrongcredentials.notactive"));
                        break;
                    case CustomerLoginResults.NotRegistered:
                        response.Messages.Add(
                            _localizationService.GetResource("account.login.wrongcredentials.notregistered"));
                        break;
                    case CustomerLoginResults.LockedOut:
                        response.Messages.Add(
                            _localizationService.GetResource("account.login.wrongcredentials.lockedout"));
                        break;
                    case CustomerLoginResults.WrongPassword:
                        response.Messages.Add(
                            _localizationService.GetResource("account.login.wrongcredentials.wrongcustomernameorpassword"));
                        break;
                    default:
                        response.Messages.Add(
                            _localizationService.GetResource("account.login.wrongcredentials"));
                        break;
                }
            }
            response.Messages.AddRange(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return Unauthorized(response);
        }


        [HttpGet]
        public bool Logout(string refreshToken)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var customerIdValue = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // The Jwt implementation does not support "revoke OAuth token" (logout) by design.
            // Delete the customer's tokens from the database (revoke its bearer token)
            _tokenStoreService.RevokeCustomerBearerTokens(customerIdValue, refreshToken);

            _eventPublisher.Publish(new CustomerLoggedOutEvent(_workContext.CurrentCustomer));

            return true;
        }

        #endregion

        #region Register / Activation

        [HttpPost]
        public virtual IActionResult Register([FromBody] RegisterRequest model)
        {
            var response = new RegisterResponse { Result = ResultType.Error };

            if (ModelState.IsValid)
            {
                model.Mobile = model.Mobile.Trim();

                var registrationRequest = new CustomerRegistrationRequest(
                    model.Mobile,
                    model.Password,
                    _customerSettings.DefaultPasswordFormat);
                var registrationResult = _customerRegistrationService.RegisterCustomer(registrationRequest);

                if (registrationResult.Success)
                {
                    //email validation message
                    var verifyCode = CommonHelper.GenerateSmsVerificationCode();
                    _genericAttributeService.SaveAttribute(registrationRequest.Customer, BopCustomerDefaults.AccountActivationTokenAttribute,
                        verifyCode);
                    //_workflowMessageService.SendcustomerPhoneValidationMessage(customer, _workContext.WorkingLanguage.Id);

                    //result
                    response.Result = ResultType.Success;

                    response.Messages.Add($"{_localizationService.GetResource("account.accountactivation.activation.code")} : {verifyCode}");
                    //raise event       
                    _eventPublisher.Publish(new CustomerRegisteredEvent(registrationRequest.Customer));
                    return Ok(response);
                }

                //errors
                foreach (var error in registrationResult.Errors)
                    response.Messages.Add(error);
            }

            response.Messages.AddRange(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return UnprocessableEntity(response);
        }

        [HttpPost]
        public virtual IActionResult Activate([FromBody] AccountActivationRequest model)
        {
            var response = new AccountActivationResponse { Result = ResultType.Error };
            var customer = _customerService.GetCustomerByPhone(model.Mobile);
            if (customer == null)
            {
                response.Messages.Add(_localizationService.GetResource("Account.AccountActivation.customerNotExist"));
                return BadRequest(response);
            }

            var customerToken = _genericAttributeService.GetAttributesForEntity(customer.Id, customer.GetType().Name)
                .SingleOrDefault(a => a.Key == BopCustomerDefaults.AccountActivationTokenAttribute);

            if (!customer.IsValidToken(customerToken, model.Code))
            {
                response.Messages.Add(_localizationService.GetResource("Account.AccountActivation.WrongToken"));
                return BadRequest(response);
            }

            //activate customer account
            customer.Active = true;
            _customerService.UpdateCustomer(customer);
            _genericAttributeService.SaveAttribute(customer, BopCustomerDefaults.AccountActivationTokenAttribute, "");

            response.Result = ResultType.Success;
            return Ok(response);
        }

        [HttpPost]
        public virtual IActionResult Resend([FromBody] AccountActivationRequest model)
        {
            var response = new AccountActivationResponse { Result = ResultType.Error };
            var customer = _customerService.GetCustomerByPhone(model.Mobile);
            if (customer == null)
            {
                response.Messages.Add(_localizationService.GetResource("account.accountactivation.customernotexist"));
                return BadRequest(response);
            }

            if (customer.Active)
            {
                response.Messages.Add(_localizationService.GetResource("account.accountactivation.customeralreadyactivated"));
                return BadRequest(response);
            }

            _genericAttributeService.SaveAttribute(customer, BopCustomerDefaults.AccountActivationTokenAttribute, "");

            var verifyCode = CommonHelper.GenerateSmsVerificationCode();
            _genericAttributeService.SaveAttribute(customer, BopCustomerDefaults.AccountActivationTokenAttribute,
                verifyCode);
            //_workflowMessageService.SendcustomerPhoneValidationMessage(customer, _workContext.WorkingLanguage.Id);

            response.Result = ResultType.Success;
            response.Messages.Add($"{_localizationService.GetResource("account.accountactivation.activation.code")} : {verifyCode}");
            return Ok(response);
        }

        #endregion
    }
}