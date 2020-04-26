using Bop.Core.Domain.Customers;

namespace Bop.Core.Domain.Security
{
    /// <summary>
    /// Represents a permission record-customer role mapping class
    /// </summary>
    public partial class PermissionRecordCustomerRoleMapping : BaseEntity
    {
        /// <summary>
        /// Gets or sets the permission record identifier
        /// </summary>
        public int PermissionRecordId { get; set; }

        /// <summary>
        /// Gets or sets the customer role identifier
        /// </summary>
        public int CustomerRoleId { get; set; }

        /// <summary>
        /// Gets or sets the permission record
        /// </summary>
        public virtual PermissionRecord PermissionRecord { get; set; }

        /// <summary>
        /// Gets or sets the customer role
        /// </summary>
        public virtual CustomerRole CustomerRole { get; set; }
    }
}