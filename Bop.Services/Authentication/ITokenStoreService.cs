using System.Threading.Tasks;
using Bop.Core.Domain.Users;

namespace Bop.Services.Authentication
{
    public interface ITokenStoreService
    {
        void AddUserToken(UserToken userToken);

        void AddUserToken(User user, string refreshTokenSerial, string accessToken, string refreshTokenSourceSerial);

        bool IsValidToken(string accessToken, int userId);

        void DeleteExpiredTokens();

        UserToken FindToken(string refreshTokenValue);

        void DeleteToken(string refreshTokenValue);

        void DeleteTokensWithSameRefreshTokenSource(string refreshTokenIdHashSource);

        void InvalidateUserTokens(int userId);


        void RevokeUserBearerTokens(string userIdValue, string refreshTokenValue);
    }
}
