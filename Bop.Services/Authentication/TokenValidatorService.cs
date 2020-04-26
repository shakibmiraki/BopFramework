using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Bop.Services.Customers;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Bop.Services.Authentication
{
    public class TokenValidatorService : ITokenValidatorService
    {
        private readonly ICustomerService _customerService;
        private readonly ITokenStoreService _tokenStoreService;

        public TokenValidatorService(ICustomerService customerService, ITokenStoreService tokenStoreService)
        {
            _customerService = customerService;
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

            var customerIdString = claimsIdentity.FindFirst(ClaimTypes.UserData).Value;
            if (!int.TryParse(customerIdString, out int customerId))
            {
                context.Fail("This is not our issued token. It has no customer-id.");
                return;
            }

            var customer = _customerService.GetCustomerById(customerId);
            if (customer == null || !customer.Active)
            {
                // customer has changed his/her password/roles/stat/IsActive
                context.Fail("This token is expired. Please login again.");
            }

            var accessToken = context.SecurityToken as JwtSecurityToken;
            if (string.IsNullOrWhiteSpace(accessToken?.RawData) || !_tokenStoreService.IsValidToken(accessToken.RawData, customerId))
            {
                context.Fail("This token is not in our database.");
            }
        }
    }
}
