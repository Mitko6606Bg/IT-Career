using System;
using System.ComponentModel.DataAnnotations;

public class LandingDateTimeAttribute : ValidationAttribute
{
    private readonly string _departureDateTimePropertyName;

    public LandingDateTimeAttribute(string departureDateTimePropertyName)
    {
        _departureDateTimePropertyName = departureDateTimePropertyName;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var departureDateTimeProperty = validationContext.ObjectType.GetProperty(_departureDateTimePropertyName);

        if (departureDateTimeProperty == null)
        {
            return new ValidationResult($"Unknown property {_departureDateTimePropertyName}");
        }

        var departureDateTimeValue = (DateTime)departureDateTimeProperty.GetValue(validationContext.ObjectInstance);
        var landingDateTimeValue = (DateTime)value;

        // Add one day to departure date
        var minLandingDateTime = departureDateTimeValue.AddDays(1);

        if (landingDateTimeValue < minLandingDateTime)
        {
            return new ValidationResult(ErrorMessage ?? $"Landing date and time must be after {minLandingDateTime}");
        }

        return ValidationResult.Success;
    }
}
