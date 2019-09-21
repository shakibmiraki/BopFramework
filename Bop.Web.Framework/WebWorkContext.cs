using System;
using System.Linq;
using Bop.Core;
using Bop.Core.Domain.Localization;
using Bop.Core.Domain.Site;
using Bop.Core.Domain.Users;
using Bop.Services.Authentication;
using Bop.Services.Localization;
using Bop.Services.Site;
using Bop.Services.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Net.Http.Headers;

namespace Bop.Web.Framework
{
    /// <summary>
    /// Represents work context for web application
    /// </summary>
    public partial class WebWorkContext : IWorkContext
    {
        #region Const

        private const string USER_COOKIE_NAME = ".Bop.User";

        #endregion

        #region Fields

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly IHostedSiteService _hostedSiteService;
        private readonly ILanguageService _languageService;
        private readonly LocalizationSettings _localizationSettings;


        private User _cachedUser;
        private HostedSite _cachedHostedSite;
        private Language _cachedLanguage;



        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="httpContextAccessor">HTTP context accessor</param>
        /// <param name="authenticationService">Authentication service</param>
        /// <param name="userService">Customer service</param>
        /// <param name="hostedSiteService"></param>
        /// <param name="languageService"></param>
        /// <param name="localizationSettings"></param>
        public WebWorkContext(IHttpContextAccessor httpContextAccessor,
            IAuthenticationService authenticationService,
            IUserService userService,
            IHostedSiteService hostedSiteService, ILanguageService languageService,LocalizationSettings localizationSettings)
        {
            _httpContextAccessor = httpContextAccessor;
            _authenticationService = authenticationService;
            _userService = userService;
            _hostedSiteService = hostedSiteService;
            _localizationSettings = localizationSettings;
            _languageService = languageService;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Get bop user cookie
        /// </summary>
        /// <returns>String value of cookie</returns>
        protected virtual string GetUserCookie()
        {
            return _httpContextAccessor.HttpContext?.Request?.Cookies[USER_COOKIE_NAME];
        }

        /// <summary>
        /// Set bop user cookie
        /// </summary>
        /// <param name="customerGuid">Guid of the customer</param>
        protected virtual void SetCustomerCookie(Guid customerGuid)
        {
            if (_httpContextAccessor.HttpContext?.Response == null)
                return;

            //delete current cookie value
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(USER_COOKIE_NAME);

            //get date of cookie expiration
            var cookieExpires = 24 * 365; //TODO make configurable
            var cookieExpiresDate = DateTime.Now.AddHours(cookieExpires);

            //if passed guid is empty set cookie as expired
            if (customerGuid == Guid.Empty)
                cookieExpiresDate = DateTime.Now.AddMonths(-1);

            //set new cookie value
            var options = new Microsoft.AspNetCore.Http.CookieOptions
            {
                HttpOnly = true,
                Expires = cookieExpiresDate
            };
            _httpContextAccessor.HttpContext.Response.Cookies.Append(USER_COOKIE_NAME, customerGuid.ToString(), options);
        }

        /// <summary>
        /// Get language from the requested page URL
        /// </summary>
        /// <returns>The found language</returns>
        protected virtual Language GetLanguageFromUrl()
        {
            if (_httpContextAccessor.HttpContext?.Request == null)
                return null;

            //whether the requsted URL is localized
            var path = _httpContextAccessor.HttpContext.Request.Path.Value;
            if (!path.IsLocalizedUrl(_httpContextAccessor.HttpContext.Request.PathBase, false, out Language language))
                return null;

            return language;
        }

        /// <summary>
        /// Get language from the request
        /// </summary>
        /// <returns>The found language</returns>
        protected virtual Language GetLanguageFromRequest()
        {
            if (_httpContextAccessor.HttpContext?.Request == null)
                return null;

            //get request culture
            var requestCulture = _httpContextAccessor.HttpContext.Features.Get<IRequestCultureFeature>()?.RequestCulture;
            if (requestCulture == null)
                return null;

            //try to get language by culture name
            var requestLanguage = _languageService.GetAllLanguages().FirstOrDefault(language =>
                language.LanguageCulture.Equals(requestCulture.Culture.Name, StringComparison.InvariantCultureIgnoreCase));

            //check language availability
            if (requestLanguage == null || !requestLanguage.Published)
                return null;

            return requestLanguage;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the current customer
        /// </summary>
        public virtual User CurrentUser
        {
            get
            {
                //whether there is a cached value
                if (_cachedUser != null)
                    return _cachedUser;

                User user = _authenticationService.GetAuthenticatedUser();
                



                if (user == null || user.Deleted || !user.Active || user.RequireReLogin)
                {
                    //get guest customer
                    var userCookie = GetUserCookie();
                    if (!string.IsNullOrEmpty(userCookie))
                    {
                        if (Guid.TryParse(userCookie, out Guid userGuid))
                        {
                            //get user from cookie (should not be registered)
                            var userByCookie = _userService.GetUserByGuid(userGuid);
                            if (userByCookie != null && !userByCookie.IsRegistered())
                                user = userByCookie;
                        }
                    }
                }

                if (user == null || user.Deleted || !user.Active || user.RequireReLogin)
                {
                    //create guest if not exists
                    user = _userService.InsertGuestUser();
                }

                if (!user.Deleted && user.Active && !user.RequireReLogin)
                {
                    //set customer cookie
                    SetCustomerCookie(user.UserGuid);

                    //cache the found customer
                    _cachedUser = user;
                }

                return _cachedUser;
            }
            set
            {
                SetCustomerCookie(value.UserGuid);
                _cachedUser = value;
            }
        }


        /// <summary>
        /// Gets or sets current user working language
        /// </summary>
        public virtual Language WorkingLanguage
        {
            get
            {
                //whether there is a cached value
                if (_cachedLanguage != null)
                    return _cachedLanguage;


                Language userLanguage = null;

                userLanguage = GetLanguageFromUrl();

                //whether we should detect the language from the request
                if (userLanguage == null && _localizationSettings.AutomaticallyDetectLanguage)
                {
                    //if not, try to get language from the request
                    userLanguage = GetLanguageFromRequest();
                }

                //cache the found language
                _cachedLanguage = userLanguage;

                return _cachedLanguage;
            }
            set => _cachedLanguage = null;
        }

        /// <summary>
        /// Gets the current store
        /// </summary>
        public virtual HostedSite CurrentSite
        {
            get
            {
                if (_cachedHostedSite != null)
                    return _cachedHostedSite;

                //try to determine the current store by HOST header
                string host = _httpContextAccessor.HttpContext?.Request?.Headers[HeaderNames.Host];

                var allHostedSites = _hostedSiteService.GetAllHostedSites();
                var site = allHostedSites.FirstOrDefault(s => _hostedSiteService.ContainsHostValue(s, host));

                if (site == null)
                {
                    //load the first found store
                    site = allHostedSites.FirstOrDefault();
                }

                _cachedHostedSite = site ?? throw new Exception("No store could be loaded");

                return _cachedHostedSite;
            }
        }

        /// <summary>
        /// Gets or sets value indicating whether we're in admin area
        /// </summary>
        public virtual bool IsAdmin { get; set; }

        #endregion
    }
}
