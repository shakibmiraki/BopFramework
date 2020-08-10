using Bop.Services.Localization;
using Bop.Web.Areas.Admin.Models;
using Bop.Web.Framework.Validators;
using FluentValidation;

namespace Bop.Web.Validator
{
    public class UpdateCustomerValidator : BaseBopValidator<ProfileRequest>
    {
        public UpdateCustomerValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage(localizationService.GetResource("Customer.Profile.FirstName.Required"));
            RuleFor(x => x.LastName).NotEmpty().WithMessage(localizationService.GetResource("Customer.Profile.LastName.Required"));
            RuleFor(x => x.Email).NotEmpty().WithMessage(localizationService.GetResource("Customer.Profile.Email.Required"));
            RuleFor(x => x.NationalCode).NotEmpty().WithMessage(localizationService.GetResource("Customer.Profile.NationalCode.Required"));
            RuleFor(x => x.BirthDate).NotEmpty().WithMessage(localizationService.GetResource("Customer.Profile.BirthDate.Required"));
            RuleFor(x => x.Gender).NotEmpty().WithMessage(localizationService.GetResource("Customer.Profile.Gender.Required"));
        }
    }
}
