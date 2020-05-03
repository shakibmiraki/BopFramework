using Bop.Core.Configuration;

namespace Bop.Core.Domain.Customers
{
    /// <summary>
    /// Customer settings
    /// </summary>
    public class CustomerSettings : ISettings
    {



        /// <summary>
        /// Gets or sets a value indicating whether users are allowed to change their usernames
        /// </summary>
        public bool AllowUsersToChangeUsernames { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether username will be validated (when registering or changing on the 'My Account' page)
        /// </summary>
        public bool UsernameValidationEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether username will be validated using regex (when registering or changing on the 'My Account' page)
        /// </summary>
        public bool UsernameValidationUseRegex { get; set; }

        /// <summary>
        /// Gets or sets a username validation rule
        /// </summary>
        public string UsernameValidationRule { get; set; }

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
        /// Gets or sets a number of minutes to lockout users (for login failures).
        /// </summary>
        public int FailedPasswordLockoutMinutes { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether customers are allowed to upload avatars.
        /// </summary>
        public bool AllowCustomersToUploadAvatars { get; set; }

        /// <summary>
        /// Gets or sets a maximum avatar size (in bytes)
        /// </summary>
        public int AvatarMaximumSizeBytes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to display default user avatar.
        /// </summary>
        public bool DefaultAvatarEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether customers location is shown
        /// </summary>
        public bool ShowCustomersLocation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show customers join date
        /// </summary>
        public bool ShowCustomersJoinDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether customers are allowed to view profiles of other customers
        /// </summary>
        public bool AllowViewingProfiles { get; set; }


        /// <summary>
        /// Gets or sets a value indicating the number of minutes for 'online customers' module
        /// </summary>
        public int OnlineCustomerMinutes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating we should store last visited page URL for each customer
        /// </summary>
        public bool StoreLastVisitedPage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating we should store IP addresses of customers
        /// </summary>
        public bool StoreIpAddresses { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the number of minutes for 'last activity' module
        /// </summary>
        public int LastActivityMinutes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether deleted customer records should be prefixed suffixed with "-DELETED"
        /// </summary>
        public bool SuffixDeletedCustomers { get; set; }

    }
}