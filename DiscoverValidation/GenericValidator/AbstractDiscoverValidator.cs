using FluentValidation;
using FluentValidation.Results;

namespace DiscoverValidation.GenericValidator
{
    public abstract class AbstractDiscoverValidator<T> : AbstractValidator<T>, IDiscoverValidator
    {
        public ValidationResult ValidateEntity(object entity)
        {
            return Validate((T) entity);
        }
    }
}