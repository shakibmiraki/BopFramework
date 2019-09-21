

namespace Bop.Core.Domain.Users
{
    /// <summary>
    /// "Customer is logged out" event
    /// </summary>
    public class UserLoggedOutEvent
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="user">Customer</param>
        public UserLoggedOutEvent(User user)
        {
            User = user;
        }

        /// <summary>
        /// Get or set the customer
        /// </summary>
        public User User { get; }
    }
}