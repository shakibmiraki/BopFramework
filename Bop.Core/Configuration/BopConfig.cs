

namespace Bop.Core.Configuration
{

    public partial class BopConfig
    {

        /// <summary>
        /// Gets or sets a value indicating whether to display the full error in production environment.
        /// It's ignored (always enabled) in development environment
        /// </summary>
        public bool DisplayFullErrorStack { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether we should use Redis server
        /// </summary>
        public bool RedisEnabled { get; set; }

        /// <summary>
        /// Gets or sets Redis connection string. Used when Redis is enabled
        /// </summary>
        public string RedisConnectionString { get; set; }

        /// <summary>
        /// Gets or sets a specific redis database; If you need to use a specific redis database, just set its number here. set NULL if should use the different database for each data type (used by default)
        /// </summary>
        public int? RedisDatabaseId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the data protection system should be configured to persist keys in the Redis database
        /// </summary>
        public bool UseRedisToStoreDataProtectionKeys { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether we should use Redis server for caching (instead of default in-memory caching)
        /// </summary>
        public bool UseRedisForCaching { get; set; }

    }
}
