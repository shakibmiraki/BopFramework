using Bop.Core.Domain.Customers;
using Bop.Services.Caching.CachingDefaults;

namespace Bop.Services.Caching.CacheEventConsumers.Customers
{
    /// <summary>
    /// Represents a customer role cache event consumer
    /// </summary>
    public partial class CustomerRoleCacheEventConsumer : CacheEventConsumer<CustomerRole>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void ClearCache(CustomerRole entity)
        {
            RemoveByPrefix(BopCustomerServiceCachingDefaults.CustomerRolesPrefixCacheKey);
            RemoveByPrefix(BopCustomerServiceCachingDefaults.CustomerCustomerRolesPrefixCacheKey, false);
        }
    }
}
