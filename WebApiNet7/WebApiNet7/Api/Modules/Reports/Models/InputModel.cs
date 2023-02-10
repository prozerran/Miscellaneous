using AutoMapper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using WebApiNet7.Api.Attributes;

namespace WebApiNet7.Api.Modules.Reports.Models
{
    public class InputModel : IValidatableObject
    {
        [Required]
        [ValidId]
        public string Id { get; init; } = string.Empty;

        [Required]
        public string Name { get; init; } = string.Empty;

        [Required]
        [GreaterThanZero]
        [DefaultValue(1000)]
        public decimal InitialCapital { get; init; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (InitialCapital > 10000000)
                yield return new ValidationResult("InitialCapital too large");
        }
    }
}
