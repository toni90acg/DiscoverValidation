using DiscoverValidation.Model.Context;
using FluentValidation.Results;

namespace DiscoverValidation.Strategy.Interface
{
    public interface IValidatableStrategy
    {
        void UpdateValidationResuls<T>(DiscoverValidatorContext context, T element, ValidationResult validationResult = null);
    }
}