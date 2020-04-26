using System;
using System.Collections.Generic;
using System.Security.Claims;
using Bop.Core.Domain.Customers;
using Bop.Services.Customers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;

namespace Bop.Services.Authentication
{
    /// <summary>
    /// Represents service using cookie middleware for the authentication
    /// </summary>
    public partial class CookieAuthenticationService : IAuthenticationService
    {
        #region Fields


        private readonly ICustomerService _customerService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private Customer _cachedCustomer;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="customerService">Customer service</param>
        /// <param name="httpContextAccessor">HTTP context accessor</param>
        public CookieAuthenticationService(ICustomerService customerService,
            IHttpContextAccessor httpContextAccessor)
        {
            _customerService = customerService;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sign in
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="isPersistent">Whether the authentication session is persisted across multiple requests</param>
        public virtual async void SignIn(Customer customer, bool isPersistent)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            //create claims for customer's customername and email
            var claims = new List<Claim>();

            if (!string.IsNullOrEmpty(customer.Username))
                claims.Add(new Claim(ClaimTypes.Name, customer.Username, ClaimValueTypes.String, BopAuthenticationDefaults.ClaimsIssuer));

            if (!string.IsNullOrEmpty(customer.Phone))
                claims.Add(new Claim(ClaimTypes.MobilePhone, customer.Phone, ClaimValueTypes.Email, BopAuthenticationDefaults.ClaimsIssuer));

            //create principal for the current authentication scheme
            var customerIdentity = new ClaimsIdentity(claims, BopAuthenticationDefaults.AuthenticationScheme);
            var customerPrincipal = new ClaimsPrincipal(customerIdentity);

            //set value indicating whether session is persisted and the time at which the authentication was issued
            var authenticationProperties = new AuthenticationProperties
            {
                IsPersistent = isPersistent,
                IssuedUtc = DateTime.UtcNow
            };

            //sign in
            await _httpContextAccessor.HttpContext.SignInAsync(BopAuthenticationDefaults.AuthenticationScheme, customerPrincipal, authenticationProperties);

            //cache authenticated customer
            _cachedCustomer = customer;
        }

        /// <summary>
        /// Sign out
        /// </summary>
        public virtual async void SignOut()
        {
            //reset cached customer
            _cachedCustomer = null;

            //and sign out from the current authentication scheme
            await _httpContextAccessor.HttpContext.SignOutAsync(BopAuthenticationDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// Get authenticated customer
        /// </summary>
        /// <returns>Customer</returns>
        public virtual Customer GetAuthenticatedCustomer()
        {
            //whether there is a cached customer
            if (_cachedCustomer != null)
                return _cachedCustomer;

            //try to get authenticated customer identity
            var authenticateResult = _httpContextAccessor.HttpContext.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme).Result;
            if (!authenticateResult.Succeeded)
                return null;

            Customer customer = null;

            //try to get customer by customername
            var customerIdClaim = authenticateResult.Principal.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier
                && claim.Issuer.Equals(BopAuthenticationDefaults.ClaimsIssuer, StringComparison.InvariantCultureIgnoreCase));
            if (customerIdClaim != null)
                customer = _customerService.GetCustomerById(int.Parse(customerIdClaim.Value));

            //whether the found customer is available
            if (customer == null || !customer.Active || customer.RequireReLogin || customer.Deleted || !customer.IsRegistered())
                return null;

            //cache authenticated customer
            _cachedCustomer = customer;

            return _cachedCustomer;
        }

        #endregion
    }
}