using Bop.Core.Caching;

namespace Bop.Services.Caching.CachingDefaults
{
    /// <summary>
    /// Represents default values related to logging services
    /// </summary>
    public static partial class BopLoggingCachingDefaults
    {
        /// <summary>
        /// Gets a key for caching
        /// </summary>
        public static CacheKey ActivityTypeAllCacheKey => new CacheKey("Bop.activitytype.all");
    }
}