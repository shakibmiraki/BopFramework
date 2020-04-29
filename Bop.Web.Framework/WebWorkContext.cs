using System;
using System.Linq;
using Bop.Core;
using Bop.Core.Domain.Localization;
using Bop.Core.Domain.Site;
using Bop.Core.Domain.Customers;
using Bop.Services.Authentication;
using Bop.Services.Localization;
using Bop.Services.Site;
using Bop.Services.Customers;
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

        private const string USER_COOKIE_NAME = ".Bop.Customer";

        #endregion

        #region Fields

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthenticationService _authenticationService;
        private readonly ICustomerService _customerService;
        private readonly IHostedSiteService _hostedSiteService;
        private readonly ILanguageService _languageService;
        private readonly LocalizationSettings _localizationSettings;


        private Customer _cachedCustomer;
        private HostedSite _cachedHostedSite;
        private Language _cachedLanguage;



        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="httpContextAccessor">HTTP context accessor</param>
        /// <param name="authenticationService">Authentication service</param>
        /// <param name="customerService">Customer service</param>
        /// <param name="hostedSiteService"></param>
        /// <param name="languageService"></param>
        /// <param name="localizationSettings"></param>
        public WebWorkContext(IHttpContextAccessor httpContextAccessor,
            IAuthenticationService authenticationService,
            ICustomerService customerService,
            IHostedSiteService hostedSiteService, ILanguageService languageService, LocalizationSettings localizationSettings)
        {
            _httpContextAccessor = httpContextAccessor;
            _authenticationService = authenticationService;
            _customerService = customerService;
            _hostedSiteService = hostedSiteService;
            _localizationSettings = localizationSettings;
            _languageService = languageService;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Get bop customer cookie
        /// </summary>
        /// <returns>String value of cookie</returns>
        protected virtual string GetCustomerCookie()
        {
            return _httpContextAccessor.HttpContext?.Request?.Cookies[USER_COOKIE_NAME];
        }

        /// <summary>
        /// Set bop customer cookie
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
            var options = new CookieOptions
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
        public virtual Customer CurrentCustomer
        {
            get
            {
                //whether there is a cached value
                if (_cachedCustomer != null)
                    return _cachedCustomer;

                Customer customer = null;

                if (customer is null || customer.Deleted || !customer.Active || customer.RequireReLogin)
                {
                    //try to get registered user
                    customer = _authenticationService.GetAuthenticatedCustomer();
                }

                if (customer != null && !customer.Deleted && customer.Active && !customer.RequireReLogin)
                {
                    //cache the found customer
                    _cachedCustomer = customer;
                }

                return _cachedCustomer;
            }
            set
            {
                _cachedCustomer = value;
            }
        }


        /// <summary>
        /// Gets or sets current customer working language
        /// </summary>
        public virtual Language WorkingLanguage
        {
            get
            {
                //whether there is a cached value
                if (_cachedLanguage != null)
                    return _cachedLanguage;
                Language customerLanguage = GetLanguageFromUrl();

                //whether we should detect the language from the request
                if (customerLanguage == null && _localizationSettings.AutomaticallyDetectLanguage)
                {
                    //if not, try to get language from the request
                    customerLanguage = GetLanguageFromRequest();
                }

                //cache the found language
                _cachedLanguage = customerLanguage;

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
