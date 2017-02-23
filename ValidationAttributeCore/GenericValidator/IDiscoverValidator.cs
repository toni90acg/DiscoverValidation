using FluentValidation.Results;

namespace ValidationAttributeCore.GenericValidator
{
    internal interface IDiscoverValidator
    {
        ValidationResult ValidateEntity(object entity);
    }
}