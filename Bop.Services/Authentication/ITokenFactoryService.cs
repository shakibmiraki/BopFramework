using Bop.Core.Domain.Users;

namespace Bop.Services.Authentication
{
    public interface ITokenFactoryService
    {
        JwtTokensData CreateJwtTokens(User user);

        string GetRefreshTokenSerial(string refreshTokenValue);
    }
}
