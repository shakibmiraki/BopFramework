namespace Bop.Services.Users
{
    /// <summary>
    /// Represents default values related to user services
    /// </summary>
    public static partial class BopUserServiceDefaults
    {

        #region User roles

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// </remarks>
        public static string UserRolesAllCacheKey => "Bop.userrole.all-{0}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : system name
        /// </remarks>
        public static string UserRolesBySystemNameCacheKey => "Bop.userrole.systemname-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string UserRolesPrefixCacheKey => "Bop.userrole.";

        #endregion

        /// <summary>
        /// Gets a key for caching current user password lifetime
        /// </summary>
        /// <remarks>
        /// {0} : user identifier
        /// </remarks>
        public static string UserPasswordLifetimeCacheKey => "Bop.users.passwordlifetime-{0}";

        /// <summary>
        /// Gets a password salt key size
        /// </summary>
        public static int PasswordSaltKeySize => 5;
        
        /// <summary>
        /// Gets a max username length
        /// </summary>
        public static int UserUsernameLength => 100;

        /// <summary>
        /// Gets a default hash format for user password
        /// </summary>
        public static string DefaultHashedPasswordFormat => "SHA512";
    }
}