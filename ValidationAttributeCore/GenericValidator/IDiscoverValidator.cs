using FluentValidation.Results;

namespace DiscoverValidationCore.GenericValidator
{
    internal interface IDiscoverValidator
    {
        ValidationResult ValidateEntity(object entity);
    }
}