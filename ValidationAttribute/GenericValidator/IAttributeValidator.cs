using FluentValidation.Results;

namespace ValidationAttribute.GenericValidator
{
    public interface IAttributeValidator
    {
        ValidationResult ValidateEntity(object entity);
    }
}