using Bop.Core.Domain.Security;
using Bop.Services.Caching;

namespace Bop.Services.Security.Caching
{
    /// <summary>
    /// Represents a permission record cache event consumer
    /// </summary>
    public partial class PermissionRecordCacheEventConsumer : CacheEventConsumer<PermissionRecord>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void ClearCache(PermissionRecord entity)
        {
            var prefix = _cacheKeyService.PrepareKeyPrefix(BopSecurityDefaults.PermissionsAllowedPrefixCacheKey, entity.SystemName);
            RemoveByPrefix(prefix);
            RemoveByPrefix(BopSecurityDefaults.PermissionsAllByCustomerRoleIdPrefixCacheKey);
        }
    }
}
