using DiscoverValidationPortable.Model.Context;
using FluentValidation.Results;

namespace DiscoverValidationPortable.Strategy.Interface
{
    public interface IValidatableStrategy
    {
        void UpdateValidationResuls<T>(DiscoverValidatorContext context, T element, ValidationResult validationResult = null);
    }
}