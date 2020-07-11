using Bop.Core.Domain.Tasks;
using Bop.Services.Caching;

namespace Bop.Services.Tasks.Caching
{
    /// <summary>
    /// Represents a schedule task cache event consumer
    /// </summary>
    public partial class ScheduleTaskCacheEventConsumer : CacheEventConsumer<ScheduleTask>
    {
    }
}
