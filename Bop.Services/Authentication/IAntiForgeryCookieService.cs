using System.Collections.Generic;
using System.Security.Claims;


namespace Bop.Services.Authentication
{
    public interface IAntiForgeryCookieService
    {
        void RegenerateAntiForgeryCookies(IEnumerable<Claim> claims);

        void DeleteAntiForgeryCookies();
    }
}
