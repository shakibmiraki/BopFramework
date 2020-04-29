using Bop.Core.Domain.Customers;


namespace Bop.Services.Customers
{
    /// <summary>
    /// Customer registration interface
    /// </summary>
    public partial interface ICustomerRegistrationService
    {
        /// <summary>
        /// Validate customer
        /// </summary>
        /// <param name="phone">Username or phone</param>
        /// <param name="password">Password</param>
        /// <returns>Result</returns>
        CustomerLoginResults ValidateCustomer(string phone, string password);

        /// <summary>
        /// Register customer
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Result</returns>
        CustomerRegistrationResult RegisterCustomer(CustomerRegistrationRequest request);

        /// <summary>
        /// Change password
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Result</returns>
        ChangePasswordResult ChangePassword(ChangePasswordRequest request);
    }
}