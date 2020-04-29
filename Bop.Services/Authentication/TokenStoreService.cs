using System;
using System.Linq;
using Bop.Core.Configuration;
using Bop.Data;
using Bop.Core.Domain.Customers;
using Bop.Services.Security;



namespace Bop.Services.Authentication
{
    public class TokenStoreService : ITokenStoreService
    {
        private readonly IEncryptionService _encryptionService;
        private readonly ITokenFactoryService _tokenFactoryService;
        private readonly IRepository<CustomerToken> _customerTokenRepository;
        private readonly JwtConfig _jwtConfig;

        public TokenStoreService(
            IEncryptionService encryptionService,
            ITokenFactoryService tokenFactoryService, 
            IRepository<CustomerToken> customerTokenRepository, 
            JwtConfig jwtConfig)
        {
            _encryptionService = encryptionService;
            _tokenFactoryService = tokenFactoryService;
            _customerTokenRepository = customerTokenRepository;
            _jwtConfig = jwtConfig;
        }

        public void AddCustomerToken(CustomerToken customerToken)
        {
            if (!_jwtConfig.AllowMultipleLoginsFromTheSameCustomer)
            {
                InvalidateCustomerTokens(customerToken.CustomerId);
            }
            DeleteTokensWithSameRefreshTokenSource(customerToken.RefreshTokenIdHashSource);
            _customerTokenRepository.Insert(customerToken);
        }

        public void AddCustomerToken(Customer customer, string refreshTokenSerial, string accessToken, string refreshTokenSourceSerial)
        {
            var now = DateTime.UtcNow;
            var token = new CustomerToken
            {
                CustomerId = customer.Id,
                // Refresh token handles should be treated as secrets and should be stored hashed
                RefreshTokenIdHash = _encryptionService.GetSha256Hash(refreshTokenSerial),
                RefreshTokenIdHashSource = string.IsNullOrWhiteSpace(refreshTokenSourceSerial) ?
                                           null : _encryptionService.GetSha256Hash(refreshTokenSourceSerial),
                AccessTokenHash = _encryptionService.GetSha256Hash(accessToken),
                RefreshTokenExpiresDateTime = now.AddMinutes(_jwtConfig.RefreshTokenExpirationMinutes),
                AccessTokenExpiresDateTime = now.AddMinutes(_jwtConfig.AccessTokenExpirationMinutes)
            };
            AddCustomerToken(token);
        }

        public void DeleteExpiredTokens()
        {
            var now = DateTimeOffset.UtcNow;
            var customerTokens = _customerTokenRepository.Table.Where(x => x.RefreshTokenExpiresDateTime < now).ToList();

            foreach (var customerToken in customerTokens)
            {
                _customerTokenRepository.Delete(customerToken);
            }
        }

        public void DeleteToken(string refreshTokenValue)
        {
            var token = FindToken(refreshTokenValue);
            if (token != null)
            {
                _customerTokenRepository.Delete(token);
            }
        }

        public void DeleteTokensWithSameRefreshTokenSource(string refreshTokenIdHashSource)
        {
            if (string.IsNullOrWhiteSpace(refreshTokenIdHashSource))
            {
                return;
            }
            var customerTokens = _customerTokenRepository.Table.Where(t => t.RefreshTokenIdHashSource == refreshTokenIdHashSource).ToList();

            foreach (var customerToken in customerTokens)
            {
                _customerTokenRepository.Delete(customerToken);
            }
        }

        public void RevokeCustomerBearerTokens(string customerIdValue, string refreshTokenValue)
        {
            if (!string.IsNullOrWhiteSpace(customerIdValue) && int.TryParse(customerIdValue, out int customerId))
            {
                if (_jwtConfig.AllowSignoutAllCustomerActiveClients)
                {
                    InvalidateCustomerTokens(customerId);
                }
            }

            if (!string.IsNullOrWhiteSpace(refreshTokenValue))
            {
                var refreshTokenSerial = _tokenFactoryService.GetRefreshTokenSerial(refreshTokenValue);
                if (!string.IsNullOrWhiteSpace(refreshTokenSerial))
                {
                    var refreshTokenIdHashSource = _encryptionService.GetSha256Hash(refreshTokenSerial);
                    DeleteTokensWithSameRefreshTokenSource(refreshTokenIdHashSource);
                }
            }

            DeleteExpiredTokens();
        }

        public CustomerToken FindToken(string refreshTokenValue)
        {
            if (string.IsNullOrWhiteSpace(refreshTokenValue))
            {
                return null;
            }

            var refreshTokenSerial = _tokenFactoryService.GetRefreshTokenSerial(refreshTokenValue);
            if (string.IsNullOrWhiteSpace(refreshTokenSerial))
            {
                return null;
            }

            var refreshTokenIdHash = _encryptionService.GetSha256Hash(refreshTokenSerial);
            return _customerTokenRepository.Table.FirstOrDefault(x => x.RefreshTokenIdHash == refreshTokenIdHash);
        }

        public void InvalidateCustomerTokens(int customerId)
        {
            var customerTokens = _customerTokenRepository.Table.Where(x => x.CustomerId == customerId).ToList();
            foreach (var customerToken in customerTokens)
            {
                _customerTokenRepository.Delete(customerToken);
            }
        }

        public bool IsValidToken(string accessToken, int customerId)
        {
            var accessTokenHash = _encryptionService.GetSha256Hash(accessToken);
            var customerToken = _customerTokenRepository.Table.FirstOrDefault(
                x => x.AccessTokenHash == accessTokenHash && x.CustomerId == customerId);
            return customerToken?.AccessTokenExpiresDateTime >= DateTime.UtcNow;
        }
    }
}
