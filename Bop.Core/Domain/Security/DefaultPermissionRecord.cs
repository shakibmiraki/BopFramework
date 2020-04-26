using System.Collections.Generic;

namespace Bop.Core.Domain.Security
{
    /// <summary>
    /// Represents a default permission record
    /// </summary>
    public class DefaultPermissionRecord
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public DefaultPermissionRecord() 
        {
            this.PermissionRecords = new List<PermissionRecord>();
        }

        /// <summary>
        /// Gets or sets the user role system name
        /// </summary>
        public string CustomerRoleSystemName { get; set; }

        /// <summary>
        /// Gets or sets the permissions
        /// </summary>
        public IEnumerable<PermissionRecord> PermissionRecords { get; set; }
    }
}
