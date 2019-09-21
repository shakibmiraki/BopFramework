using Bop.Core.Domain.Users;

namespace Bop.Services.Authentication
{
    /// <summary>
    /// Authentication service interface
    /// </summary>
    public interface IAuthenticationService 
    {
        /// <summary>
        /// Sign in
        /// </summary>
        /// <param name="user">Customer</param>
        /// <param name="isPersistent">Whether the authentication session is persisted across multiple requests</param>
        void SignIn(User user, bool isPersistent);

        /// <summary>
        /// Sign out
        /// </summary>
        void SignOut();

        /// <summary>
        /// Get authenticated customer
        /// </summary>
        /// <returns>Customer</returns>
        User GetAuthenticatedUser();
    }
}