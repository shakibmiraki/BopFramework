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
        /// <param name="customernameOrEmail">Customername or email</param>
        /// <param name="password">Password</param>
        /// <returns>Result</returns>
        CustomerLoginResults ValidateCustomer(string customernameOrEmail, string password);

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

        /// <summary>
        /// Sets a customer email
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="newEmail">New email</param>
        /// <param name="requireValidation">Require validation of new email address</param>
        void SetPhone(Customer customer, string newEmail, bool requireValidation);

        /// <summary>
        /// Sets a customer customername
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="newCustomername">New Customername</param>
        void SetUsername(Customer customer, string newCustomername);
    }
}