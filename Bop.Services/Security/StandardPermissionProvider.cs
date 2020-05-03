using System.Collections.Generic;
using Bop.Core.Domain.Customers;
using Bop.Core.Domain.Security;

namespace Bop.Services.Security
{
    /// <summary>
    /// Standard permission provider
    /// </summary>
    public partial class StandardPermissionProvider : IPermissionProvider
    {
        //admin area permissions
        public static readonly PermissionRecord AccessAdminPanel = new PermissionRecord { Name = "Access admin area", SystemName = "AccessAdminPanel", Category = "Standard" };
        public static readonly PermissionRecord ManageCustomers = new PermissionRecord { Name = "Admin area. Manage Customers", SystemName = "ManageCustomers", Category = "Customers" };
        public static readonly PermissionRecord ManageLanguages = new PermissionRecord { Name = "Admin area. Manage Languages", SystemName = "ManageLanguages", Category = "Localization" };

        /// <summary>
        /// Get permissions
        /// </summary>
        /// <returns>Permissions</returns>
        public virtual IEnumerable<PermissionRecord> GetPermissions()
        {
            return new[]
            {
                AccessAdminPanel,
                ManageCustomers,
                ManageLanguages
            };
        }

        /// <summary>
        /// Get default permissions
        /// </summary>
        /// <returns>Permissions</returns>
        public virtual HashSet<(string systemRoleName, PermissionRecord[] permissions)> GetDefaultPermissions()
        {
            return new HashSet<(string, PermissionRecord[])>
            {
                (
                    BopCustomerDefaults.AdministratorsRoleName,
                    new[]
                    {
                        AccessAdminPanel,
                        ManageCustomers,
                        ManageLanguages
                    }
                ),
                (
                    BopCustomerDefaults.RegisteredRoleName,
                    new[]
                    {
                        AccessAdminPanel
                    }
                )
            };
        }
    }
}