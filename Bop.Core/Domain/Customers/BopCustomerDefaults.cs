namespace Bop.Core.Domain.Customers
{
    /// <summary>
    /// Represents default values related to customers data
    /// </summary>
    public static partial class BopCustomerDefaults
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


        #endregion


        #region Customer attributes

        /// <summary>
        /// Gets a name of generic attribute to store the value of 'AccountActivationToken'
        /// </summary>
        public static string AccountActivationTokenAttribute => "AccountActivationToken";

        public static string FirstName => "FirstName";

        public static string LastName => "LastName";

        public static string Email => "Email";

        public static string NationalCode => "NationalCode";

        public static string BirthDate => "BirthDate";

        public static string Gender => "Gender";

        #endregion

    }
}