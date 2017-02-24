using FluentValidation;
using FluentValidation.Results;

namespace DiscoverValidationPortable.GenericValidator
{
    public abstract class AbstractDiscoverValidator<T> : AbstractValidator<T>, IDiscoverValidator
    {
        public ValidationResult ValidateEntity(object entity)
        {
            return Validate((T) entity);
        }
    }
}