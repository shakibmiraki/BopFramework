
namespace Bop.Core.Domain.Customers
{
    /// <summary>
    /// Represents default values related to customers data
    /// </summary>
    public static class BopCustomerDefaults
    {
        #region System customer roles

        /// <summary>
        /// Gets a system name of 'administrators' customer role
        /// </summary>
        public static string AdministratorsRoleName => "Administrators";

        /// <summary>
        /// Gets a system name of 'registered' customer role
        /// </summary>
        public static string RegisteredRoleName => "Registered";

        /// <summary>
        /// Gets a system name of 'guests' customer role
        /// </summary>
        public static string GuestsRoleName => "Guests";


        #endregion

        #region Customer attributes

        /// <summary>
        /// Gets a name of generic attribute to store the value of 'AccountActivationToken'
        /// </summary>
        public static string AccountActivationTokenAttribute => "AccountActivationToken";

        #endregion
    }
}