using Bop.Core.Domain.Logging;
using Bop.Services.Caching;

namespace Bop.Services.Logging.Caching
{
    /// <summary>
    /// Represents a log cache event consumer
    /// </summary>
    public partial class LogCacheEventConsumer : CacheEventConsumer<Log>
    {
    }
}
