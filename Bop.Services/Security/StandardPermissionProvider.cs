using System.Collections.Generic;
using Bop.Core.Domain.Security;
using Bop.Core.Domain.Users;


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
        public static readonly PermissionRecord ManageLanguages = new PermissionRecord { Name = "Admin area. Manage Languages", SystemName = "ManageLanguages", Category = "Configuration" };
        public static readonly PermissionRecord ManageSettings = new PermissionRecord { Name = "Admin area. Manage Settings", SystemName = "ManageSettings", Category = "Configuration" };
        public static readonly PermissionRecord ManageScheduleTasks = new PermissionRecord { Name = "Admin area. Manage Schedule Tasks", SystemName = "ManageScheduleTasks", Category = "Configuration" };


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
                ManageLanguages,
                ManageSettings,
                ManageScheduleTasks
            };
        }

        /// <summary>
        /// Get default permissions
        /// </summary>
        /// <returns>Permissions</returns>
        public virtual IEnumerable<DefaultPermissionRecord> GetDefaultPermissions()
        {
            return new[] 
            {
                new DefaultPermissionRecord 
                {
                    UserRoleSystemName = BopUserDefaults.AdministratorsRoleName,
                    PermissionRecords = new[] 
                    {
                        AccessAdminPanel,
                        ManageCustomers,
                        ManageLanguages,
                        ManageSettings,
                        ManageScheduleTasks
                    }
                },
                new DefaultPermissionRecord 
                {
                    UserRoleSystemName = BopUserDefaults.RegisteredRoleName,
                    PermissionRecords = new[] 
                    {
                        AccessAdminPanel
                    }
                }
            };
        }
    }
}