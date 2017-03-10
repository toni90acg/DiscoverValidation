using DiscoverValidation.Model.Context;
using FluentValidation.Results;

namespace DiscoverValidation.Strategy.Interface
{
    public interface IValidatableStrategy
    {
        void UpdateValidationResuls<T>(DiscoverValidatorContext context, T element, ValidationResult validationResult = null);
        void UpdateValidationResulsLock<T>(DiscoverValidatorContext context, T element, object lockObject, ValidationResult validationResult = null);
    }
}