using Bop.Web.Framework.Models;


namespace Bop.Web.Models.Customers
{
    public class LoginRequest : BaseModel
    {

        public string Phone { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}