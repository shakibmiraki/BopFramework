using System;
using System.Linq;
using System.Security.Claims;
using Bop.Core;
using Bop.Core.Domain.Users;
using Bop.Data.Extensions;
using Bop.Services.Authentication;
using Bop.Services.Common;
using Bop.Services.Events;
using Bop.Services.Localization;
using Bop.Services.Messages;
using Bop.Services.Users;
using Bop.Web.Framework.Controllers;
using Bop.Web.Framework.UI;
using Bop.Web.Models.Users;
using Microsoft.AspNetCore.Mvc;



namespace Bop.Web.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IUserRegistrationService _userRegistrationService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly ITokenStoreService _tokenStoreService;
        private readonly ITokenFactoryService _tokenFactoryService;
        private readonly UserSettings _userSettings;
        private readonly IAntiForgeryCookieService _antiForgeryCookieService;

        public UserController(IUserService userService,
            IUserRegistrationService userRegistrationService,
            IAuthenticationService authenticationService,
            IEventPublisher eventPublisher,
            ILocalizationService localizationService,
            IWorkContext workContext,
            IGenericAttributeService genericAttributeService,
            IWorkflowMessageService workflowMessageService,
            ITokenStoreService tokenStoreService,
            ITokenFactoryService tokenFactoryService,
            UserSettings userSettings, IAntiForgeryCookieService antiForgeryCookieService)
        {
            _userService = userService;
            _userRegistrationService = userRegistrationService;
            _authenticationService = authenticationService;
            _eventPublisher = eventPublisher;
            _localizationService = localizationService;
            _workContext = workContext;
            _genericAttributeService = genericAttributeService;
            _workflowMessageService = workflowMessageService;
            _tokenStoreService = tokenStoreService;
            _tokenFactoryService = tokenFactoryService;
            _userSettings = userSettings;
            _antiForgeryCookieService = antiForgeryCookieService;
        }



        #region Login / Logout

        [HttpPost]
        public IActionResult Login([FromBody]LoginRequest model)
        {
            var response = new LoginResponse { Result = ResultType.Error };

            if (ModelState.IsValid)
            {
                model.Phone = model.Phone.Trim();
                var loginResult = _userRegistrationService.ValidateUser(model.Phone, model.Password);

                switch (loginResult)
                {
                    case UserLoginResults.Successful:
                        {
                            var user = _userService.GetUserByPhone(model.Phone);

                            //sign in new customer
                            var token = _tokenFactoryService.CreateJwtTokens(user);
                            _tokenStoreService.AddUserToken(user, token.RefreshTokenSerial, token.AccessToken, null);

                            //raise event       
                            _eventPublisher.Publish(new UserLoggedinEvent(user));

                            response.Result = ResultType.Success;
                            response.AccessToken = token.AccessToken;
                            response.RefreshToken = token.RefreshToken;

                            return Ok(response);
                        }
                    case UserLoginResults.UserNotExist:
                        response.Messages.Add(
                            _localizationService.GetResource("account.login.wrongcredentials.customernotexist"));
                        break;
                    case UserLoginResults.Deleted:
                        response.Messages.Add(
                            _localizationService.GetResource("account.login.wrongcredentials.deleted"));
                        break;
                    case UserLoginResults.NotActive:
                        response.Messages.Add(
                            _localizationService.GetResource("account.login.wrongcredentials.notactive"));
                        break;
                    case UserLoginResults.NotRegistered:
                        response.Messages.Add(
                            _localizationService.GetResource("account.login.wrongcredentials.notregistered"));
                        break;
                    case UserLoginResults.LockedOut:
                        response.Messages.Add(
                            _localizationService.GetResource("account.login.wrongcredentials.lockedout"));
                        break;
                    case UserLoginResults.WrongPassword:
                        response.Messages.Add(
                            _localizationService.GetResource("account.login.wrongcredentials.wrongusernameorpassword"));
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
            var userIdValue = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // The Jwt implementation does not support "revoke OAuth token" (logout) by design.
            // Delete the user's tokens from the database (revoke its bearer token)
            _tokenStoreService.RevokeUserBearerTokens(userIdValue, refreshToken);

            _antiForgeryCookieService.DeleteAntiForgeryCookies();

            _eventPublisher.Publish(new UserLoggedOutEvent(_workContext.CurrentUser));

            return true;
        }

        #endregion

        #region Register / Activation

        [HttpPost]
        public virtual IActionResult Register([FromBody]RegisterRequest model)
        {
            var response = new RegisterResponse { Result = ResultType.Error };
            if (_workContext.CurrentUser.IsRegistered())
            {
                //raise logged out event       
                _eventPublisher.Publish(new UserLoggedOutEvent(_workContext.CurrentUser));

                //Save a new record
                _workContext.CurrentUser = _userService.InsertGuestUser();
            }
            var user = _workContext.CurrentUser;

            if (ModelState.IsValid)
            {
                model.Phone = model.Phone.Trim();

                var registrationRequest = new UserRegistrationRequest(user,
                    model.Phone,
                    model.Password,
                    _userSettings.DefaultPasswordFormat);
                var registrationResult = _userRegistrationService.RegisterUser(registrationRequest);
                if (registrationResult.Success)
                {
                    //email validation message
                    var verifyCode = CommonHelper.GenerateSmsVerificationCode();
                    _genericAttributeService.SaveAttribute(user, BopUserDefaults.AccountActivationTokenAttribute,
                        verifyCode);
                    //_workflowMessageService.SendUserPhoneValidationMessage(user, _workContext.WorkingLanguage.Id);

                    //result
                    response.Result = ResultType.Success;

                    response.Messages.Add($"{_localizationService.GetResource("account.accountactivation.activation.code")} : {verifyCode}");
                    //raise event       
                    _eventPublisher.Publish(new UserRegisteredEvent(user));
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
        public virtual IActionResult Activate([FromBody]AccountActivationRequest model)
        {
            var response = new AccountActivationResponse { Result = ResultType.Error };
            var user = _userService.GetUserByPhone(model.Phone);
            if (user == null)
            {
                response.Messages.Add(_localizationService.GetResource("Account.AccountActivation.UserNotExist"));
                return BadRequest(response);
            }

            //var cToken = _genericAttributeService.GetAttribute<string>(user, BopUserDefaults.AccountActivationTokenAttribute);

            var cToken = _genericAttributeService.GetAttributesForEntity(user.Id, user.GetUnproxiedEntityType().Name)
                .SingleOrDefault(a => a.Key == BopUserDefaults.AccountActivationTokenAttribute);

            if (cToken is null || string.IsNullOrEmpty(cToken.Value) || cToken.InsertDate < DateTime.Now.AddMinutes(-2))
            {
                response.Messages.Add(_localizationService.GetResource("Account.AccountActivation.TokenExpired"));
                return BadRequest(response);
            }

            if (!cToken.Value.Equals(model.Token, StringComparison.InvariantCultureIgnoreCase))
            {
                response.Messages.Add(_localizationService.GetResource("Account.AccountActivation.WrongToken"));
                return BadRequest(response);
            }

            //activate user account
            user.Active = true;
            _userService.UpdateUser(user);
            _genericAttributeService.SaveAttribute(user, BopUserDefaults.AccountActivationTokenAttribute, "");

            response.Result = ResultType.Success;
            return Ok(response);
        }

        [HttpPost]
        public virtual IActionResult Resend([FromBody]AccountActivationRequest model)
        {
            var response = new AccountActivationResponse { Result = ResultType.Error };
            var user = _userService.GetUserByPhone(model.Phone);
            if (user == null)
            {
                response.Messages.Add(_localizationService.GetResource("account.accountactivation.usernotexist"));
                return BadRequest(response);
            }

            if (user.Active)
            {
                response.Messages.Add(_localizationService.GetResource("account.accountactivation.useralreadyactivated"));
                return BadRequest(response);
            }

            _genericAttributeService.SaveAttribute(user, BopUserDefaults.AccountActivationTokenAttribute, "");

            var verifyCode = CommonHelper.GenerateSmsVerificationCode();
            _genericAttributeService.SaveAttribute(user, BopUserDefaults.AccountActivationTokenAttribute,
                verifyCode);
            //_workflowMessageService.SendUserPhoneValidationMessage(customer, _workContext.WorkingLanguage.Id);

            response.Result = ResultType.Success;
            response.Messages.Add($"{_localizationService.GetResource("account.accountactivation.activation.code")} : {verifyCode}");
            return Ok(response);
        }

        #endregion
    }
}