

namespace Bop.Core.Configuration
{
    public class JwtConfig
    {

        public string Key { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public int AccessTokenExpirationMinutes { get; set; }

        public int RefreshTokenExpirationMinutes { get; set; }

        public bool AllowMultipleLoginsFromTheSameCustomer { get; set; }

        public bool AllowSignoutAllCustomerActiveClients { get; set; }
    }
}
