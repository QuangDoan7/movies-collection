using System.ComponentModel.DataAnnotations;

namespace movies_collection.Models;

public class LastWatchedYearValidation : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success;
        }

        int lastWatchedYear = Convert.ToInt32(value);
        
        var movie = (Movie)validationContext.ObjectInstance;

        var releaseYear = movie.ReleaseYear;
        var currentYear = DateTime.Now.Year;

        if (lastWatchedYear >= releaseYear && lastWatchedYear <= currentYear)
        {
            return ValidationResult.Success;
        }

        return new ValidationResult(
            $"Last Watched Year must be between the release year {releaseYear} and the current year {currentYear}."
        );
    }
}
