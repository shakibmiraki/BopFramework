
namespace Bop.Data.Mapping
{
    /// <summary>
    /// Represents default values related to data mapping
    /// </summary>
    public static partial class BopMappingDefaults
    {

        /// <summary>
        /// Gets a name of the User-UserRole mapping table
        /// </summary>
        public static string UserUserRoleTable => "User_UserRole_Mapping";


        /// <summary>
        /// Gets a name of the User-UserCard mapping table
        /// </summary>
        public static string UserUserCardTable => "User_UserCard_Mapping";

        /// <summary>
        /// Gets a name of the PermissionRecord-UserRole mapping table
        /// </summary>
        public static string PermissionRecordRoleTable => "PermissionRecord_Role_Mapping";


    }
}