using Bop.Core.Domain.Localization;
using Bop.Services.Caching;

namespace Bop.Services.Localization.Caching
{
    /// <summary>
    /// Represents a localized property cache event consumer
    /// </summary>
    public partial class LocalizedPropertyCacheEventConsumer : CacheEventConsumer<LocalizedProperty>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void ClearCache(LocalizedProperty entity)
        {
            Remove(BopLocalizationDefaults.LocalizedPropertyAllCacheKey);

            var cacheKey = _cacheKeyService.PrepareKey(BopLocalizationDefaults.LocalizedPropertyCacheKey,
                entity.LanguageId, entity.EntityId, entity.LocaleKeyGroup, entity.LocaleKey);

            Remove(cacheKey);
        }
    }
}
