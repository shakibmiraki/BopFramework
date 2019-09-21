using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Bop.Services.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Bop.Services.Authentication
{
    public class TokenValidatorService : ITokenValidatorService
    {
        private readonly IUserService _userService;
        private readonly ITokenStoreService _tokenStoreService;

        public TokenValidatorService(IUserService userService, ITokenStoreService tokenStoreService)
        {
            _userService = userService;
            _tokenStoreService = tokenStoreService;
        }

        public async Task ValidateAsync(TokenValidatedContext context)
        {
            var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
            if (claimsIdentity?.Claims == null || !claimsIdentity.Claims.Any())
            {
                context.Fail("This is not our issued token. It has no claims.");
                return;
            }

            var phoneNumberClaim = claimsIdentity.FindFirst(ClaimTypes.MobilePhone);
            if (phoneNumberClaim == null)
            {
                context.Fail("This is not our issued token. It has no phone.");
                return;
            }

            var userIdString = claimsIdentity.FindFirst(ClaimTypes.UserData).Value;
            if (!int.TryParse(userIdString, out int userId))
            {
                context.Fail("This is not our issued token. It has no user-id.");
                return;
            }

            var user = _userService.GetUserById(userId);
            if (user == null || !user.Active)
            {
                // user has changed his/her password/roles/stat/IsActive
                context.Fail("This token is expired. Please login again.");
            }

            var accessToken = context.SecurityToken as JwtSecurityToken;
            if (string.IsNullOrWhiteSpace(accessToken?.RawData) || !_tokenStoreService.IsValidToken(accessToken.RawData, userId))
            {
                context.Fail("This token is not in our database.");
            }
        }
    }
}
