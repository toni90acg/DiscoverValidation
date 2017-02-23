using FluentValidation.Results;
using ValidationAttributeCore.Model.Context;

namespace ValidationAttributeCore.Strategy.Interface
{
    public interface IValidatableStrategy
    {
        void UpdateValidationResuls<T>(DiscoverValidatorContext context, T element, ValidationResult validationResult = null);
    }
}