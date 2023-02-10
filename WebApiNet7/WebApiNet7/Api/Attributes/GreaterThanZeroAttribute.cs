using System.ComponentModel.DataAnnotations;

namespace WebApiNet7.Api.Attributes
{
    public class GreaterThanZeroAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult($"{validationContext.MemberName} can't be null");

            return value switch
            {
                int _ => Convert.ToInt32(value) > 0
                    ? ValidationResult.Success
                    : new ValidationResult($"{validationContext.MemberName} must be greater than zero"),

                decimal _ => Convert.ToDecimal(value) > 0
                    ? ValidationResult.Success
                    : new ValidationResult($"{validationContext.MemberName} must be greater than zero"),

                double _ => Convert.ToDouble(value) > 0
                    ? ValidationResult.Success
                    : new ValidationResult($"{validationContext.MemberName} must be greater than zero"),

                long _ => Convert.ToInt64(value) > 0
                    ? ValidationResult.Success
                    : new ValidationResult($"{validationContext.MemberName} must be greater than zero"),

                _ => new ValidationResult($"{validationContext.MemberName} validation type {value.GetType().Name} not supported for this attribute")
            };
        }
    }
}
