using FluentValidation.Results;

namespace DiscoverValidation.GenericValidator
{
    public interface IDiscoverValidator
    {
        ValidationResult ValidateEntity(object entity);
    }
}