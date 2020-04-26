using System;
using System.Linq;
using Bop.Core;
using Bop.Core.Domain.Site;
using Bop.Services.Site;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Bop.Services.Common;

namespace Nop.Web.Framework
{
    /// <summary>
    /// Store context for web application
    /// </summary>
    public partial class HostedSiteContext : IHostedSiteContext
    {
        #region Fields

        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostedSiteService _hostedSiteService;

        private HostedSite _cachedHostedSite;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="genericAttributeService">Generic attribute service</param>
        /// <param name="httpContextAccessor">HTTP context accessor</param>
        /// <param name="storeService">Store service</param>
        public HostedSiteContext(IGenericAttributeService genericAttributeService,
            IHttpContextAccessor httpContextAccessor,
            IHostedSiteService hostedSiteService)
        {
            _genericAttributeService = genericAttributeService;
            _httpContextAccessor = httpContextAccessor;
            _hostedSiteService = hostedSiteService;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the current store
        /// </summary>
        public virtual HostedSite CurrentHostedSite
        {
            get
            {
                if (_cachedHostedSite != null)
                    return _cachedHostedSite;

                //try to determine the current store by HOST header
                string host = _httpContextAccessor.HttpContext?.Request?.Headers[HeaderNames.Host];

                var allStores = _hostedSiteService.GetAllHostedSites();
                var store = allStores.FirstOrDefault(s => _hostedSiteService.ContainsHostValue(s, host));

                if (store == null)
                {
                    //load the first found store
                    store = allStores.FirstOrDefault();
                }

                _cachedHostedSite = store ?? throw new Exception("No store could be loaded");

                return _cachedHostedSite;
            }
        }

        #endregion
    }
}