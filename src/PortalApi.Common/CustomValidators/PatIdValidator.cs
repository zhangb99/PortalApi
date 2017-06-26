using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Validators;

namespace PortalApi.Common
{
    public class PatIdValidator<T> : PropertyValidator
    {
        public PatIdValidator()
            : base("{PropertyName} must be a valid PatId")
        {

        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            return Regex.IsMatch(context.PropertyValue.ToString(), @"^(z|Z)\d{1,23}$");
        }
    }

    public static class PatIdValidatorExtensions
    {
        public static IRuleBuilderOptions<T, TElement> IsPatId<T, TElement>(this IRuleBuilder<T, TElement> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new PatIdValidator<TElement>());
        }
    }
}
