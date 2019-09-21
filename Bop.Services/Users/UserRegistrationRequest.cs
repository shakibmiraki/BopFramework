using Bop.Core.Domain.Users;


namespace Bop.Services.Users
{
    /// <summary>
    /// Customer registration request
    /// </summary>
    public class UserRegistrationRequest
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="user">Customer</param>
        /// <param name="phone">Email</param>
        /// <param name="password">Password</param>
        /// <param name="passwordFormat">Password format</param>
        public UserRegistrationRequest(User user,string phone ,
            string password,
            PasswordFormat passwordFormat)
        {
            User = user;
            Password = password;
            PasswordFormat = passwordFormat;
            Phone = phone;
        }

        /// <summary>
        /// Customer
        /// </summary>
        public User User { get; set; }

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
