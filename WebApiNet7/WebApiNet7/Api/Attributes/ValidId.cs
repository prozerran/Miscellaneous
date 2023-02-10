using System.ComponentModel.DataAnnotations;

namespace WebApiNet7.Api.Attributes
{
    public class ValidId : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult($"{validationContext.MemberName} can't be null");

            return value switch
            {
                string stringValue => long.TryParse(stringValue, out _)
                    ? ValidationResult.Success
                    : new ValidationResult($"{validationContext.MemberName} is not a valid Id"),

                _ => new ValidationResult($"{validationContext.MemberName} validation type {value.GetType().Name} not supported for this attribute")
            };
        }
    }
}
