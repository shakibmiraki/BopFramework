namespace Bop.Services.Customers
{
    /// <summary>
    /// Represents default values related to customer services
    /// </summary>
    public static partial class BopCustomerServiceDefaults
    {

        #region Customer roles

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// </remarks>
        public static string CustomerRolesAllCacheKey => "Bop.customerrole.all-{0}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : system name
        /// </remarks>
        public static string CustomerRolesBySystemNameCacheKey => "Bop.customerrole.systemname-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string CustomerRolesPrefixCacheKey => "Bop.customerrole.";

        #endregion

        /// <summary>
        /// Gets a key for caching current customer password lifetime
        /// </summary>
        /// <remarks>
        /// {0} : customer identifier
        /// </remarks>
        public static string CustomerPasswordLifetimeCacheKey => "Bop.customers.passwordlifetime-{0}";

        /// <summary>
        /// Gets a password salt key size
        /// </summary>
        public static int PasswordSaltKeySize => 5;
        
        /// <summary>
        /// Gets a max customername length
        /// </summary>
        public static int CustomerCustomernameLength => 100;

        /// <summary>
        /// Gets a default hash format for customer password
        /// </summary>
        public static string DefaultHashedPasswordFormat => "SHA512";
    }
}