﻿namespace Bop.Services.Defaults
{
    /// <summary>
    /// Represents default values related to customer services
    /// </summary>
    public static partial class BopCustomerServiceDefaults
    {
        /// <summary>
        /// Gets a password salt key size
        /// </summary>
        public static int PasswordSaltKeySize => 5;
        
        /// <summary>
        /// Gets a max username length
        /// </summary>
        public static int CustomerUsernameLength => 100;

        /// <summary>
        /// Gets a default hash format for customer password
        /// </summary>
        public static string DefaultHashedPasswordFormat => "SHA512";
    }
}