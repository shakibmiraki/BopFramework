using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Bop.Core.Configuration;
using Bop.Core.Domain.Users;
using Bop.Services.Security;
using Bop.Services.Users;
using Microsoft.IdentityModel.Tokens;

namespace Bop.Services.Authentication
{

    public class JwtTokensData
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public string RefreshTokenSerial { get; set; }

        public IEnumerable<Claim> Claims { get; set; }
    }


    public class TokenFactoryService : ITokenFactoryService
    {
        private readonly IEncryptionService _encryptionService;
        private readonly IUserService _userService;
        private readonly ILogger _logger;
        private readonly JwtConfig _jwtConfig;

        public TokenFactoryService(
            IEncryptionService encryptionService,
            IUserService userService,
            ILogger logger,
            JwtConfig jwtConfig)
        {
            _encryptionService = encryptionService;
            _userService = userService;
            _logger = logger;
            _jwtConfig = jwtConfig;
        }


        public JwtTokensData CreateJwtTokens(User user)
        {
            var (accessToken, claims) = CreateAccessToken(user);
            var (refreshTokenValue, refreshTokenSerial) = CreateRefreshToken();
            return new JwtTokensData
            {
                AccessToken = accessToken,
                RefreshToken = refreshTokenValue,
                RefreshTokenSerial = refreshTokenSerial,
                Claims = claims
            };
        }

        private (string RefreshTokenValue, string RefreshTokenSerial) CreateRefreshToken()
        {
            var refreshTokenSerial = _encryptionService.CreateCryptographicallySecureGuid().ToString().Replace("-", "");

            var claims = new List<Claim>
            {
                // Unique Id for all Jwt tokes
                new Claim(JwtRegisteredClaimNames.Jti, _encryptionService.CreateCryptographicallySecureGuid().ToString(), ClaimValueTypes.String, _jwtConfig.Issuer),
                // Issuer
                new Claim(JwtRegisteredClaimNames.Iss, _jwtConfig.Issuer, ClaimValueTypes.String, _jwtConfig.Issuer),
                // Issued at
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64, _jwtConfig.Issuer),
                // for invalidation
                new Claim(ClaimTypes.SerialNumber, refreshTokenSerial, ClaimValueTypes.String, _jwtConfig.Issuer)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var now = DateTime.UtcNow;
            var token = new JwtSecurityToken(
                issuer: _jwtConfig.Issuer,
                audience: _jwtConfig.Audience,
                claims: claims,
                notBefore: now,
                expires: now.AddMinutes(_jwtConfig.RefreshTokenExpirationMinutes),
                signingCredentials: creds);
            var refreshTokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            return (refreshTokenValue, refreshTokenSerial);
        }

        public string GetRefreshTokenSerial(string refreshTokenValue)
        {
            if (string.IsNullOrWhiteSpace(refreshTokenValue))
            {
                return null;
            }

            ClaimsPrincipal decodedRefreshTokenPrincipal = null;
            try
            {
                decodedRefreshTokenPrincipal = new JwtSecurityTokenHandler().ValidateToken(
                    refreshTokenValue,
                    new TokenValidationParameters
                    {
                        RequireExpirationTime = true,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Key)),
                        ValidateIssuerSigningKey = true, // verify signature to avoid tampering
                        ValidateLifetime = true, // validate the expiration
                        ClockSkew = TimeSpan.Zero // tolerance for the expiration date
                    },
                    out _
                );
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to validate refreshTokenValue: `{refreshTokenValue}`.", ex);
            }

            return decodedRefreshTokenPrincipal?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.SerialNumber)?.Value;
        }

        private (string AccessToken, IEnumerable<Claim> Claims) CreateAccessToken(User user)
        {
            var claims = new List<Claim>
            {
                // Unique Id for all Jwt tokes
                new Claim(JwtRegisteredClaimNames.Jti,
                    _encryptionService.CreateCryptographicallySecureGuid().ToString(), ClaimValueTypes.String,
                    _jwtConfig.Issuer),
                // Issuer
                new Claim(JwtRegisteredClaimNames.Iss, _jwtConfig.Issuer, ClaimValueTypes.String, _jwtConfig.Issuer),
                // Issued at
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                    ClaimValueTypes.Integer64, _jwtConfig.Issuer),

                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), ClaimValueTypes.String, _jwtConfig.Issuer),

                new Claim(ClaimTypes.MobilePhone, user.Phone, ClaimValueTypes.String, _jwtConfig.Issuer),
                // custom data
                new Claim(ClaimTypes.UserData, user.Id.ToString(), ClaimValueTypes.String, _jwtConfig.Issuer),

                new Claim(ClaimTypes.Name, BopAuthenticationDefaults.ClaimsIssuer, ClaimValueTypes.String,
                    _jwtConfig.Issuer)
            };

            // add roles
            var userRoles = _userService.GetUserById(user.Id).UserRoles;
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole.SystemName, ClaimValueTypes.String, _jwtConfig.Issuer));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var now = DateTime.UtcNow;
            var token = new JwtSecurityToken(
                issuer: _jwtConfig.Issuer,
                audience: _jwtConfig.Audience,
                claims: claims,
                notBefore: now,
                expires: now.AddMinutes(_jwtConfig.AccessTokenExpirationMinutes),
                signingCredentials: creds);
            return (new JwtSecurityTokenHandler().WriteToken(token), claims);
        }
    }
}
