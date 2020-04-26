using Bop.Core.Caching;


namespace Bop.Services.Caching.CachingDefaults
{
    /// <summary>
    /// Represents default values related to configuration services
    /// </summary>
    public static partial class BopConfigurationCachingDefaults
    {
        /// <summary>
        /// Gets a key for caching
        /// </summary>
        public static CacheKey SettingsAllAsDictionaryCacheKey => new CacheKey("Bop.setting.all.as.dictionary", SettingsPrefixCacheKey);

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        public static CacheKey SettingsAllCacheKey => new CacheKey("Bop.setting.all", SettingsPrefixCacheKey);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string SettingsPrefixCacheKey => "Bop.setting.";
    }
}