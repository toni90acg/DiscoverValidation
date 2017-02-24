using DiscoverValidationPortable.Helpers;
using DiscoverValidationPortable.Model;
using DiscoverValidationPortable.Model.Context;
using DiscoverValidationPortable.Strategy.Interface;
using FluentValidation.Results;

namespace DiscoverValidationPortable.Strategy.Strategies
{
    public class CreateValidDataStrategy : IValidatableStrategy
    {
        public void UpdateValidationResuls<T>(DiscoverValidatorContext context, T element, ValidationResult validationResult = null)
        {
            var data = CreateInstanceFactory.CreateData(typeof(ValidData<>), element);
            context.DiscoverValidationResults.ValidDataList.Add(data);
            context.DiscoverValidationResults.AllDataList.Add(data);
        }
    }
}