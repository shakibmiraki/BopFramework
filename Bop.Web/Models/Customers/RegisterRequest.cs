using Bop.Web.Framework.Models;


namespace Bop.Web.Models.Customers
{
    public class RegisterRequest : BaseModel
    {


        public string Mobile { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

    }
}
