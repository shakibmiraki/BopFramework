using Bop.Core.Configuration;


namespace Bop.Core.Domain.Customers
{
    /// <summary>
    /// Customer settings
    /// </summary>
    public class CustomerSettings : ISettings
    {

        /// <summary>
        /// Default password format for customers
        /// </summary>
        public PasswordFormat DefaultPasswordFormat { get; set; }

        /// <summary>
        /// Gets or sets a customer password format (SHA1, MD5) when passwords are hashed (DO NOT edit in production environment)
        /// </summary>
        public string HashedPasswordFormat { get; set; }

        /// <summary>
        /// Gets or sets a minimum password length
        /// </summary>
        public int PasswordMinLength { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether password are have least one lowercase
        /// </summary>
        public bool PasswordRequireLowercase { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether password are have least one uppercase
        /// </summary>
        public bool PasswordRequireUppercase { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether password are have least one non alphanumeric character
        /// </summary>
        public bool PasswordRequireNonAlphanumeric { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether password are have least one digit
        /// </summary>
        public bool PasswordRequireDigit { get; set; }

        /// <summary>
        /// Gets or sets a number of passwords that should not be the same as the previous one; 0 if the customer can use the same password time after time
        /// </summary>
        public int UnduplicatedPasswordsNumber { get; set; }

        /// <summary>
        /// Gets or sets a number of days for password recovery link. Set to 0 if it doesn't expire.
        /// </summary>
        public int PasswordRecoveryLinkDaysValid { get; set; }

        /// <summary>
        /// Gets or sets a number of days for password expiration
        /// </summary>
        public int PasswordLifetime { get; set; }

        /// <summary>
        /// Gets or sets maximum login failures to lockout account. Set 0 to disable this feature
        /// </summary>
        public int FailedPasswordAllowedAttempts { get; set; }

        /// <summary>
        /// Gets or sets a number of minutes to lockout customers (for login failures).
        /// </summary>
        public int FailedPasswordLockoutMinutes { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether deleted customer records should be prefixed suffixed with "-DELETED"
        /// </summary>
        public bool SuffixDeletedCustomers { get; set; }


        /// <summary>
        /// Gets or sets interval (in minutes) with which the Delete Guest Task runs
        /// </summary>
        public int DeleteGuestTaskOlderThanMinutes { get; set; }


    }
}