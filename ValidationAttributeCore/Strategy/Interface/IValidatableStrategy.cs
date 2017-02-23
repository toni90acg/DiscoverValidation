using DiscoverValidationCore.Model.Context;
using FluentValidation.Results;

namespace DiscoverValidationCore.Strategy.Interface
{
    public interface IValidatableStrategy
    {
        void UpdateValidationResuls<T>(DiscoverValidatorContext context, T element, ValidationResult validationResult = null);
    }
}