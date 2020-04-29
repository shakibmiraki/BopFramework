

namespace Bop.Services.Authentication
{
    /// <summary>
    /// Represents default values related to authentication services
    /// </summary>
    public static class BopAuthenticationDefaults
    {
        /// <summary>
        /// The default value used for authentication scheme
        /// </summary>
        public static string AuthenticationScheme => "Bearer";

        /// <summary>
        /// The issuer that should be used for any claims that are created
        /// </summary>
        public static string ClaimsIssuer => "Bop";
    }
}