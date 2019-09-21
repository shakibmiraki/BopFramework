using Bop.Services.Localization;
using Bop.Web.Framework.Validators;
using Bop.Web.Models.Users;
using FluentValidation;

namespace Bop.Web.Validator
{
    public class UserRegisterValidator : BaseBopValidator<RegisterRequest>
    {
        public UserRegisterValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Phone).NotEmpty()
                .WithMessage(localizationService.GetResource("Authentication.Register.Phone.Required"))
                .Matches(@"^(09)[13][0-9]\d{7}$")
                .WithMessage(localizationService.GetResource("Authentication.Register.Phone.BadFormat"));

            RuleFor(x => x.Password).NotEmpty()
                .WithMessage(localizationService.GetResource("Authentication.Register.Password.Required"))
                .MinimumLength(6)
                .WithMessage(localizationService.GetResource("Authentication.Register.Password.MinimumLength"));

            RuleFor(x => x.Password).Equal(x => x.ConfirmPassword).WithMessage("Authentication.Register.Password.NotEqual");

        }
    }
}
