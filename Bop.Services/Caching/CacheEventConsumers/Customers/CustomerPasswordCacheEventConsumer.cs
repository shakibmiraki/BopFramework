using Bop.Core.Domain.Customers;

namespace Bop.Services.Caching.CacheEventConsumers.Customers
{
    /// <summary>
    /// Represents a customer password cache event consumer
    /// </summary>
    public partial class CustomerPasswordCacheEventConsumer : CacheEventConsumer<CustomerPassword>
    {
    }
}