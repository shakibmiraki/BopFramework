using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Bop.Services.Authentication
{
    public interface ITokenValidatorService
    {
        Task ValidateAsync(TokenValidatedContext context);
    }
}
