using Bop.Core.Domain.Users;

namespace Bop.Core.Domain.Users
{
    /// <summary>
    /// Customer registered event
    /// </summary>
    public class UserRegisteredEvent
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="user">customer</param>
        public UserRegisteredEvent(User user)
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