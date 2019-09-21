
namespace Bop.Core.Http
{
    /// <summary>
    /// Represents default values related to HTTP features
    /// </summary>
    public static class BopHttpDefaults
    {
        /// <summary>
        /// Gets the name of the default HTTP client
        /// </summary>
        public static string DefaultHttpClient => "default";


        /// <summary>
        /// Gets the name of HTTP_CLUSTER_HTTPS header
        /// </summary>
        public static string HttpClusterHttpsHeader => "HTTP_CLUSTER_HTTPS";

        /// <summary>
        /// Gets the name of HTTP_X_FORWARDED_PROTO header
        /// </summary>
        public static string HttpXForwardedProtoHeader => "X-Forwarded-Proto";

    }
}