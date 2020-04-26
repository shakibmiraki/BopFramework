using Bop.Core.Domain.Customers;

namespace Bop.Services.Authentication
{
    public interface ITokenFactoryService
    {
        JwtTokensData CreateJwtTokens(Customer customer);

        string GetRefreshTokenSerial(string refreshTokenValue);
    }
}
