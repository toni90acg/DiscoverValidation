using FluentValidation;
using FluentValidation.Results;

namespace ValidationAttributeCore.GenericValidator
{
    public abstract class AbstractDiscoverValidator<T> : AbstractValidator<T>, IDiscoverValidator
    {
        public ValidationResult ValidateEntity(object entity)
        {
            return Validate((T) entity);
        }
    }
}