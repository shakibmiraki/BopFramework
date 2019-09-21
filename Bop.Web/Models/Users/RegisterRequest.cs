using Bop.Web.Framework.Models;


namespace Bop.Web.Models.Users
{
  public class RegisterRequest : BaseModel
    {
       

        public string Phone { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public bool DisplayCaptcha { get; set; }
    }
}
