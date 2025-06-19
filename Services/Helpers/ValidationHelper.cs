using System.ComponentModel.DataAnnotations;

namespace Services.Helpers;

public class ValidationHelper
{
    internal static void ModelValidation(object obj)
    {
        // Validate the person name using Model Validation
        ValidationContext validationContext = new ValidationContext(obj);
        List<ValidationResult> validationResults = new();
        bool isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);
        if (!isValid)
        {
            // If validation fails, throw an exception with the validation errors
            throw new ArgumentException(validationResults.FirstOrDefault()?.ErrorMessage);
        }
    }
}
