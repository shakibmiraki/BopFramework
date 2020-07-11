using Bop.Core.Domain.Configuration;
using Bop.Services.Caching;

namespace Bop.Services.Configuration.Caching
{
    /// <summary>
    /// Represents a setting cache event consumer
    /// </summary>
    public partial class SettingCacheEventConsumer : CacheEventConsumer<Setting>
    {
    }
}