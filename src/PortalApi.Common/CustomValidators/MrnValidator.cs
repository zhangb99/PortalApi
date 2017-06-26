using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Validators;

namespace PortalApi.Common
{
    public class MrnValidator<T> : PropertyValidator
    {
        public MrnValidator()
            : base("{PropertyName} must be a valid 8-digit MRN")
        {

        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            return Regex.IsMatch(context.PropertyValue.ToString(), @"^\d{8,8}$");
        }
    }

    public static class MrnValidatorExtensions
    {
        public static IRuleBuilderOptions<T, TElement> IsMrn<T, TElement>(this IRuleBuilder<T, TElement> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new MrnValidator<TElement>());
        }
    }
}
