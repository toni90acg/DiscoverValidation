using FluentValidation.Results;

namespace DiscoverValidationCore.GenericValidator
{
    public interface IDiscoverValidator
    {
        ValidationResult ValidateEntity(object entity);
    }
}