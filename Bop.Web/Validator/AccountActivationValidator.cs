using Bop.Services.Localization;
using Bop.Web.Framework.Validators;
using Bop.Web.Models.Customers;
using FluentValidation;

namespace Bop.Web.Validator
{
    public class AccountActivationValidator : BaseBopValidator<AccountActivationRequest>
    {
        public AccountActivationValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Mobile).NotEmpty()
                .WithMessage(localizationService.GetResource("Authentication.Activate.Phone.Required"))
                .Matches(@"^(09)[13][0-9]\d{7}$")
                .WithMessage(localizationService.GetResource("Authentication.Activate.Phone.BadFormat"));

            RuleFor(x => x.Code).NotEmpty()
                .WithMessage(localizationService.GetResource("Authentication.Activate.Token.Required"))
                .Length(5)
                .WithMessage(localizationService.GetResource("Authentication.Activate.Token.Length"));
        }
    }
}
