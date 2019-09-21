using System;
using System.Linq;
using Bop.Core.Configuration;
using Bop.Core.Data;
using Bop.Core.Domain.Users;
using Bop.Services.Security;



namespace Bop.Services.Authentication
{
    public class TokenStoreService : ITokenStoreService
    {
        private readonly IEncryptionService _encryptionService;
        private readonly ITokenFactoryService _tokenFactoryService;
        private readonly IRepository<UserToken> _userTokenRepository;
        private readonly JwtConfig _jwtConfig;

        public TokenStoreService(
            IEncryptionService encryptionService,
            ITokenFactoryService tokenFactoryService, 
            IRepository<UserToken> userTokenRepository, 
            JwtConfig jwtConfig)
        {
            _encryptionService = encryptionService;
            _tokenFactoryService = tokenFactoryService;
            _userTokenRepository = userTokenRepository;
            _jwtConfig = jwtConfig;
        }

        public void AddUserToken(UserToken userToken)
        {
            if (!_jwtConfig.AllowMultipleLoginsFromTheSameUser)
            {
                InvalidateUserTokens(userToken.UserId);
            }
            DeleteTokensWithSameRefreshTokenSource(userToken.RefreshTokenIdHashSource);
            _userTokenRepository.Insert(userToken);
        }

        public void AddUserToken(User user, string refreshTokenSerial, string accessToken, string refreshTokenSourceSerial)
        {
            var now = DateTimeOffset.UtcNow;
            var token = new UserToken
            {
                UserId = user.Id,
                // Refresh token handles should be treated as secrets and should be stored hashed
                RefreshTokenIdHash = _encryptionService.GetSha256Hash(refreshTokenSerial),
                RefreshTokenIdHashSource = string.IsNullOrWhiteSpace(refreshTokenSourceSerial) ?
                                           null : _encryptionService.GetSha256Hash(refreshTokenSourceSerial),
                AccessTokenHash = _encryptionService.GetSha256Hash(accessToken),
                RefreshTokenExpiresDateTime = now.AddMinutes(_jwtConfig.RefreshTokenExpirationMinutes),
                AccessTokenExpiresDateTime = now.AddMinutes(_jwtConfig.AccessTokenExpirationMinutes)
            };
            AddUserToken(token);
        }

        public void DeleteExpiredTokens()
        {
            var now = DateTimeOffset.UtcNow;
            var userTokens = _userTokenRepository.Table.Where(x => x.RefreshTokenExpiresDateTime < now).ToList();

            foreach (var userToken in userTokens)
            {
                _userTokenRepository.Delete(userToken);
            }
        }

        public void DeleteToken(string refreshTokenValue)
        {
            var token = FindToken(refreshTokenValue);
            if (token != null)
            {
                _userTokenRepository.Delete(token);
            }
        }

        public void DeleteTokensWithSameRefreshTokenSource(string refreshTokenIdHashSource)
        {
            if (string.IsNullOrWhiteSpace(refreshTokenIdHashSource))
            {
                return;
            }
            var userTokens = _userTokenRepository.Table.Where(t => t.RefreshTokenIdHashSource == refreshTokenIdHashSource).ToList();

            foreach (var userToken in userTokens)
            {
                _userTokenRepository.Delete(userToken);
            }
        }

        public void RevokeUserBearerTokens(string userIdValue, string refreshTokenValue)
        {
            if (!string.IsNullOrWhiteSpace(userIdValue) && int.TryParse(userIdValue, out int userId))
            {
                if (_jwtConfig.AllowSignoutAllUserActiveClients)
                {
                    InvalidateUserTokens(userId);
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

        public UserToken FindToken(string refreshTokenValue)
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
            return _userTokenRepository.Table.FirstOrDefault(x => x.RefreshTokenIdHash == refreshTokenIdHash);
        }

        public void InvalidateUserTokens(int userId)
        {
            var userTokens = _userTokenRepository.Table.Where(x => x.UserId == userId).ToList();
            foreach (var userToken in userTokens)
            {
                _userTokenRepository.Delete(userToken);
            }
        }

        public bool IsValidToken(string accessToken, int userId)
        {
            var accessTokenHash = _encryptionService.GetSha256Hash(accessToken);
            var userToken = _userTokenRepository.Table.FirstOrDefault(
                x => x.AccessTokenHash == accessTokenHash && x.UserId == userId);
            return userToken?.AccessTokenExpiresDateTime >= DateTimeOffset.UtcNow;
        }
    }
}
