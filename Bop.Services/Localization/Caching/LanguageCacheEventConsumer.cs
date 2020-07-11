using Bop.Core.Domain.Localization;
using Bop.Services.Caching;

namespace Bop.Services.Localization.Caching
{
    /// <summary>
    /// Represents a language cache event consumer
    /// </summary>
    public partial class LanguageCacheEventConsumer : CacheEventConsumer<Language>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void ClearCache(Language entity)
        {
            Remove(_cacheKeyService.PrepareKey(BopLocalizationDefaults.LocaleStringResourcesAllPublicCacheKey, entity));
            Remove(_cacheKeyService.PrepareKey(BopLocalizationDefaults.LocaleStringResourcesAllAdminCacheKey, entity));
            Remove(_cacheKeyService.PrepareKey(BopLocalizationDefaults.LocaleStringResourcesAllCacheKey, entity));

            var prefix = _cacheKeyService.PrepareKeyPrefix(BopLocalizationDefaults.LocaleStringResourcesByResourceNamePrefixCacheKey, entity);
            RemoveByPrefix(prefix);

            RemoveByPrefix(BopLocalizationDefaults.LanguagesAllPrefixCacheKey);
        }
    }
}