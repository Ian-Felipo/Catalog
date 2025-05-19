using System.ComponentModel.DataAnnotations;

namespace CatalogApi.Validations;

public class FirstCapitalLetterAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrEmpty(value.ToString()))
        {
            return ValidationResult.Success;
        }

        var firstCapital = value.ToString()[0].ToString();

        if (firstCapital != firstCapital.ToUpper())
        {
            return new ValidationResult("A primeira letra deve ser maiuscula!");
        }

        return ValidationResult.Success;
    }
}