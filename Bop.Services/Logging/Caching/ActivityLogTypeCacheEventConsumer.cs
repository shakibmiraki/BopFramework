using Bop.Core.Domain.Logging;
using Bop.Services.Caching;

namespace Bop.Services.Logging.Caching
{
    /// <summary>
    /// Represents a activity log type cache event consumer
    /// </summary>
    public partial class ActivityLogTypeCacheEventConsumer : CacheEventConsumer<ActivityLogType>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void ClearCache(ActivityLogType entity)
        {
            Remove(BopLoggingDefaults.ActivityTypeAllCacheKey);
        }
    }
}
