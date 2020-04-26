using Bop.Core.Domain.Customers;
using Bop.Services.Caching.CachingDefaults;

namespace Bop.Services.Caching.CacheEventConsumers.Customers
{
    /// <summary>
    /// Represents a customer customer role mapping cache event consumer
    /// </summary>
    public partial class CustomerCustomerRoleMappingCacheEventConsumer : CacheEventConsumer<CustomerCustomerRoleMapping>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void ClearCache(CustomerCustomerRoleMapping entity)
        {
            RemoveByPrefix(BopCustomerServiceCachingDefaults.CustomerCustomerRolesPrefixCacheKey, false);
        }
    }
}