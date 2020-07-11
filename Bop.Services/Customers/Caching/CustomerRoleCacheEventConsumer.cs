using Bop.Core.Domain.Customers;
using Bop.Services.Caching;

namespace Bop.Services.Customers.Caching
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
            RemoveByPrefix(BopCustomerServicesDefaults.CustomerRolesPrefixCacheKey);
            RemoveByPrefix(BopCustomerServicesDefaults.CustomerCustomerRolesPrefixCacheKey);
        }
    }
}
