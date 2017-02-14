using FluentValidation.Results;

namespace ValidationAttribute.GenericValidator
{
    public interface IAttributeValidator
    {
        /// <summary>
        /// Validate the entity if it has the MyValidationAttribute with a 
        /// type of validation (with FluentValidation)
        /// </summary>
        /// <param name="entity">Entity to validate</param>
        /// <returns>Validation results</returns>
        ValidationResult ValidateEntity(object entity);
    }
}