using Bop.Services.Localization;
using Bop.Web.Framework.Validators;
using Bop.Web.Models.Users;
using FluentValidation;

namespace Bop.Web.Validator
{

    public class LoginValidator : BaseBopValidator<LoginRequest>
    {
        public LoginValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Phone).NotEmpty()
                .WithMessage(localizationService.GetResource("Authentication.Login.Phone.Required"))
                .Matches(@"^(09)[13][0-9]\d{7}$")
                .WithMessage(localizationService.GetResource("Authentication.Login.Phone.BadFormat"));

            RuleFor(x => x.Password).NotEmpty()
                .WithMessage(localizationService.GetResource("Authentication.Login.Password.Required"))
                .MinimumLength(6)
                .WithMessage(localizationService.GetResource("Authentication.Login.Password.MinimumLength"));
        }
    }
}
