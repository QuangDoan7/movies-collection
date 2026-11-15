using System.ComponentModel.DataAnnotations;

namespace movies_collection.Models;

public class ReleaseYearValidation : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success;
        }

        int releaseYear = Convert.ToInt32(value);

        var movie = (Movie)validationContext.ObjectInstance;

        var currentYear = DateTime.Now.Year;

        if (releaseYear >= 1900 && releaseYear <= currentYear)
        {
            return ValidationResult.Success;
        }

        return new ValidationResult(
            $"Release Year must be between 1900 and the current year {currentYear}."
        );
    }
}
