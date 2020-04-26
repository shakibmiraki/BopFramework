using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Bop.Core.Configuration;
using Bop.Core.Http;
using Bop.Core.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

namespace Bop.Core
{
    public class WebHelper : IWebHelper
    {
        #region Fields 

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HostingConfig _hostingConfig;

        #endregion

        #region Ctor

        public WebHelper(IHttpContextAccessor httpContextAccessor, HostingConfig hostingConfig)
        {
            _httpContextAccessor = httpContextAccessor;
            _hostingConfig = hostingConfig;
        }

        #endregion

        #region Utilities

        protected virtual bool IsRequestAvailable()
        {
            if (_httpContextAccessor?.HttpContext == null)
                return false;

            try
            {
                if (_httpContextAccessor.HttpContext.Request == null)
                    return false;
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        #endregion

        #region Methods

        public virtual string GetCurrentIpAddress()
        {
            if (!IsRequestAvailable())
                return string.Empty;

            string result;
            try
            {
                result = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            }
            catch
            {
                return string.Empty;
            }

            //some of the validation
            if (!string.IsNullOrEmpty(result) && result.Equals(IPAddress.IPv6Loopback.ToString(), StringComparison.InvariantCultureIgnoreCase))
                result = IPAddress.Loopback.ToString();

            //"TryParse" doesn't support IPv4 with port number
            if (IPAddress.TryParse(result ?? string.Empty, out var ip))
                //IP address is valid 
                result = ip.ToString();
            else if (!string.IsNullOrEmpty(result))
                //remove port
                result = result.Split(':').FirstOrDefault();

            return result;
        }

        public virtual string GetUrlReferrer()
        {
            if (!IsRequestAvailable())
                return string.Empty;

            //URL referrer is null in some case (for example, in IE 8)
            return _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Referer];
        }

        public virtual string GetThisPageUrl(bool includeQueryString, bool? useSsl = null, bool lowercaseUrl = false)
        {
            if (!IsRequestAvailable())
                return string.Empty;

            //add local path to the URL
            var pageUrl = $"{_httpContextAccessor.HttpContext.Request.Path}";

            //add query string to the URL
            if (includeQueryString)
                pageUrl = $"{pageUrl}{_httpContextAccessor.HttpContext.Request.QueryString}";

            //whether to convert the URL to lower case
            if (lowercaseUrl)
                pageUrl = pageUrl.ToLowerInvariant();

            return pageUrl;
        }

        /// <summary>
        /// Gets hostedSite location
        /// </summary>
        /// <param name="useSsl">Whether to get SSL secured URL; pass null to determine automatically</param>
        /// <returns>Store location</returns>
        public virtual string GetSiteLocation(bool? useSsl = null)
        {
            var siteLocation = string.Empty;

            //get store host
            var siteHost = GetSiteHost(useSsl ?? IsCurrentConnectionSecured());
            if (!string.IsNullOrEmpty(siteHost))
            {
                //add application path base if exists
                siteLocation = IsRequestAvailable() ? $"{siteHost.TrimEnd('/')}{_httpContextAccessor.HttpContext.Request.PathBase}" : siteHost;
            }

            //if host is empty (it is possible only when HttpContext is not available), use URL of a store entity configured in admin area
            if (string.IsNullOrEmpty(siteHost))
            {
                //do not inject IWorkContext via constructor because it'll cause circular references
                siteLocation = EngineContext.Current.Resolve<IHostedSiteContext>().CurrentHostedSite?.Url
                    ?? throw new Exception("Current store cannot be loaded");
            }

            //ensure that URL is ended with slash
            siteLocation = $"{siteLocation.TrimEnd('/')}/";

            return siteLocation;
        }


        /// <summary>
        /// Returns true if the requested resource is one of the typical resources that needn't be processed by the cms engine.
        /// </summary>
        /// <returns>True if the request targets a static resource file.</returns>
        public virtual bool IsStaticResource()
        {
            if (!IsRequestAvailable())
                return false;

            string path = _httpContextAccessor.HttpContext.Request.Path;

            //a little workaround. FileExtensionContentTypeProvider contains most of static file extensions. So we can use it
            //source: https://github.com/aspnet/StaticFiles/blob/dev/src/Microsoft.AspNetCore.StaticFiles/FileExtensionContentTypeProvider.cs
            //if it can return content type, then it's a static file
            var contentTypeProvider = new FileExtensionContentTypeProvider();
            return contentTypeProvider.TryGetContentType(path, out var _);
        }

        /// <summary>
        /// Gets store host location
        /// </summary>
        /// <param name="useSsl">Whether to get SSL secured URL</param>
        /// <returns>Store host location</returns>
        public virtual string GetSiteHost(bool useSsl)
        {
            if (!IsRequestAvailable())
                return string.Empty;

            //try to get host from the request HOST header
            var hostHeader = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Host];
            if (StringValues.IsNullOrEmpty(hostHeader))
                return string.Empty;

            //add scheme to the URL
            var siteHost = $"{(useSsl ? Uri.UriSchemeHttps : Uri.UriSchemeHttp)}{Uri.SchemeDelimiter}{hostHeader.FirstOrDefault()}";

            //ensure that host is ended with slash
            siteHost = $"{siteHost.TrimEnd('/')}/";

            return siteHost;
        }


        /// <summary>
        /// Gets a value indicating whether current connection is secured
        /// </summary>
        /// <returns>True if it's secured, otherwise false</returns>
        public virtual bool IsCurrentConnectionSecured()
        {
            if (!IsRequestAvailable())
                return false;

            //check whether hosting uses a load balancer
            //use HTTP_CLUSTER_HTTPS?
            if (_hostingConfig.UseHttpClusterHttps)
                return _httpContextAccessor.HttpContext.Request.Headers[BopHttpDefaults.HttpClusterHttpsHeader].ToString().Equals("on", StringComparison.OrdinalIgnoreCase);

            //use HTTP_X_FORWARDED_PROTO?
            if (_hostingConfig.UseHttpXForwardedProto)
                return _httpContextAccessor.HttpContext.Request.Headers[BopHttpDefaults.HttpXForwardedProtoHeader].ToString().Equals("https", StringComparison.OrdinalIgnoreCase);

            return _httpContextAccessor.HttpContext.Request.IsHttps;
        }

        public virtual T QueryString<T>(string name)
        {
            if (!IsRequestAvailable())
                return default(T);

            if (StringValues.IsNullOrEmpty(_httpContextAccessor.HttpContext.Request.Query[name]))
                return default(T);

            return CommonHelper.To<T>(_httpContextAccessor.HttpContext.Request.Query[name].ToString());
        }

        public StringContent CreateJson<T>(T model)
        {
            var json = JsonConvert.SerializeObject(model);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        #endregion
    }
}
