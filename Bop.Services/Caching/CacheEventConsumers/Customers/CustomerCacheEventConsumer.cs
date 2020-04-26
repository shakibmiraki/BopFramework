using Bop.Core.Domain.Customers;
using Bop.Services.Caching.CachingDefaults;
using Bop.Services.Events;

namespace Bop.Services.Caching.CacheEventConsumers.Customers
{
    /// <summary>
    /// Represents a customer cache event consumer
    /// </summary>
    public partial class CustomerCacheEventConsumer : CacheEventConsumer<Customer>, IConsumer<CustomerPasswordChangedEvent>
    {
        #region Methods

        /// <summary>
        /// Handle password changed event
        /// </summary>
        /// <param name="eventMessage">Event message</param>
        public void HandleEvent(CustomerPasswordChangedEvent eventMessage)
        {
            Remove(BopCustomerServiceCachingDefaults.CustomerPasswordLifetimeCacheKey.FillCacheKey(eventMessage.Password.CustomerId));
        }

        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void ClearCache(Customer entity)
        {
            RemoveByPrefix(BopCustomerServiceCachingDefaults.CustomerCustomerRolesPrefixCacheKey, false);
            RemoveByPrefix(BopCustomerServiceCachingDefaults.CustomerAddressesPrefixCacheKey, false);
        }

        #endregion
    }
}