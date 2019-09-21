using FluentValidation;

namespace Bop.Web.Framework.Validators
{
    public abstract class BaseBopValidator<TModel> : AbstractValidator<TModel> where TModel : class
    {
    }
}
