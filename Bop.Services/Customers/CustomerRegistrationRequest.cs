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
        public CustomerRegistrationRequest(Customer customer,string phone ,
            string password,
            PasswordFormat passwordFormat)
        {
            Customer = customer;
            Password = password;
            PasswordFormat = passwordFormat;
            Phone = phone;
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
        /// Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Password format
        /// </summary>
        public PasswordFormat PasswordFormat { get; set; }

        /// <summary>
        /// Is approved
        /// </summary>
        public bool IsApproved { get; set; }
    }
}
