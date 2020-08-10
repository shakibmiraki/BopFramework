using Bop.Core.Domain.Customers;

namespace Bop.Services.Customers
{
    /// <summary>
    /// Customer registration request
    /// </summary>
    public class CustomerRegistrationRequest
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="phone">Email</param>
        /// <param name="password">Password</param>
        /// <param name="passwordFormat">Password format</param>
        /// <param name="isActive">Is approved</param>
        public CustomerRegistrationRequest(string phone,
            string password,
            PasswordFormat passwordFormat,
            bool isActive = false)
        {
            Customer = new Customer
            {
                Mobile = phone,
                Username = phone,
                Active = isActive,
            };
            Phone = phone;
            Password = password;
            PasswordFormat = passwordFormat;
            IsApproved = isActive;
        }

        /// <summary>
        /// Customer
        /// </summary>
        public Customer Customer { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Password format
        /// </summary>
        public PasswordFormat PasswordFormat { get; set; }

        /// <summary>
        /// Store identifier
        /// </summary>
        public int StoreId { get; set; }

        /// <summary>
        /// Is approved
        /// </summary>
        public bool IsApproved { get; set; }
    }
}
