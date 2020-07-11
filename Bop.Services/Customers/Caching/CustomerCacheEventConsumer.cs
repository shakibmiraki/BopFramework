using Bop.Core.Domain.Customers;
using Bop.Services.Caching;
using Bop.Services.Events;


namespace Bop.Services.Customers.Caching
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
            Remove(_cacheKeyService.PrepareKey(BopCustomerServicesDefaults.CustomerPasswordLifetimeCacheKey, eventMessage.Password.CustomerId));
        }

        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void ClearCache(Customer entity)
        {
            RemoveByPrefix(BopCustomerServicesDefaults.CustomerCustomerRolesPrefixCacheKey);
            RemoveByPrefix(BopCustomerServicesDefaults.CustomerAddressesPrefixCacheKey);
        }

        #endregion
    }
}