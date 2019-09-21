namespace Bop.Services.Site
{
    /// <summary>
    /// Represents default values related to hosted sites services
    /// </summary>
    public static partial class BopHostedSiteDefaults
    {
        #region Stores

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        public static string HostedSitesAllCacheKey => "Bop.hostedsites.all";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : store ID
        /// </remarks>
        public static string HostedSitesByIdCacheKey => "Bop.hostedsites.id-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string HostedSitesPrefixCacheKey => "Bop.hostedsites.";

        #endregion
    }
}