using System.ComponentModel.DataAnnotations;

namespace Common.Validation.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class GuidNotEmptyAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is Guid guid && guid != Guid.Empty)
            return ValidationResult.Success;
        
        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
    }
}
