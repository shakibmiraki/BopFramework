using Bop.Core.Configuration;

namespace Bop.Core.Caching
{
    /// <summary>
    /// Caching settings
    /// </summary>
    public class CachingSettings : ISettings
    {
        /// <summary>
        /// Gets or sets the default cache time in minutes
        /// </summary>
        public int DefaultCacheTime { get; set; }

        /// <summary>
        /// Gets or sets the short term cache time in minutes
        /// </summary>
        public int ShortTermCacheTime { get; set; }

        /// <summary>
        /// Gets or sets the bundled files cache time in minutes
        /// </summary>
        public int BundledFilesCacheTime { get; set; }
    }
}
