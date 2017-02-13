using FluentValidation;
using FluentValidation.Results;

namespace ValidationAttribute.GenericValidator
{
    public class AbstractAttributeValidator<T> : AbstractValidator<T>, IAttributeValidator
    {
        public ValidationResult ValidateEntity(object entity)
        {
            return Validate((T) entity);
        }
    }
}