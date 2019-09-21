

namespace Bop.Core.Domain.Users
{
    /// <summary>
    /// Customer logged-in event
    /// </summary>
    public class UserLoggedinEvent
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="user">Customer</param>
        public UserLoggedinEvent(User user)
        {
            User = user;
        }

        /// <summary>
        /// Customer
        /// </summary>
        public User User
        {
            get;
        }
    }
}