using Bop.Core.Domain.Security;
using Bop.Services.Caching;

namespace Bop.Services.Security.Caching
{
    /// <summary>
    /// Represents a permission record-customer role mapping cache event consumer
    /// </summary>
    public partial class PermissionRecordCustomerRoleMappingCacheEventConsumer : CacheEventConsumer<PermissionRecordCustomerRoleMapping>
    {
    }
}