using Bop.Web.Framework.Models;

namespace Bop.Web.Models.Users
{
    public class LoginResponse : BaseResponseModel
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
