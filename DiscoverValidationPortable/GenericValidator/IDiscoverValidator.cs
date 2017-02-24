using FluentValidation.Results;

namespace DiscoverValidationPortable.GenericValidator
{
    public interface IDiscoverValidator
    {
        ValidationResult ValidateEntity(object entity);
    }
}