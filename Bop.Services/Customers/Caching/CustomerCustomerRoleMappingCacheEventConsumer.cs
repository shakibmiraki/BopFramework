using Bop.Core.Domain.Customers;
using Bop.Services.Caching;

namespace Bop.Services.Customers.Caching
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
            RemoveByPrefix(BopCustomerServicesDefaults.CustomerCustomerRolesPrefixCacheKey);
        }
    }
}