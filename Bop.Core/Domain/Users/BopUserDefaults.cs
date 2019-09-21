
namespace Bop.Core.Domain.Users
{
    /// <summary>
    /// Represents default values related to users data
    /// </summary>
    public static class BopUserDefaults
    {
        #region System user roles

        /// <summary>
        /// Gets a system name of 'administrators' user role
        /// </summary>
        public static string AdministratorsRoleName => "Administrators";

        /// <summary>
        /// Gets a system name of 'registered' user role
        /// </summary>
        public static string RegisteredRoleName => "Registered";

        /// <summary>
        /// Gets a system name of 'guests' user role
        /// </summary>
        public static string GuestsRoleName => "Guests";


        #endregion

        #region User attributes

        /// <summary>
        /// Gets a name of generic attribute to store the value of 'AccountActivationToken'
        /// </summary>
        public static string AccountActivationTokenAttribute => "AccountActivationToken";

        #endregion
    }
}