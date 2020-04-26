using Bop.Core.Domain.Customers;

namespace Bop.Services.Authentication
{
    public interface ITokenStoreService
    {
        void AddCustomerToken(CustomerToken customerToken);

        void AddCustomerToken(Customer customer, string refreshTokenSerial, string accessToken, string refreshTokenSourceSerial);

        bool IsValidToken(string accessToken, int customerId);

        void DeleteExpiredTokens();

        CustomerToken FindToken(string refreshTokenValue);

        void DeleteToken(string refreshTokenValue);

        void DeleteTokensWithSameRefreshTokenSource(string refreshTokenIdHashSource);

        void InvalidateCustomerTokens(int customerId);


        void RevokeCustomerBearerTokens(string customerIdValue, string refreshTokenValue);
    }
}
