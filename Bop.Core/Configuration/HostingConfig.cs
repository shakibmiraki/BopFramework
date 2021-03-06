namespace Bop.Core.Configuration
{
    /// <summary>
    /// Represents startup hosting configuration parameters
    /// </summary>
    public partial class HostingConfig
    {

        /// <summary>
        /// Gets or sets a value indicating whether to use HTTP_CLUSTER_HTTPS
        /// </summary>
        public bool UseHttpClusterHttps { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use HTTP_X_FORWARDED_PROTO
        /// </summary>
        public bool UseHttpXForwardedProto { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether to use HTTP_X_FORWARDED_PROTO
        /// </summary>
        public string CorsPath { get; set; }
    }
}