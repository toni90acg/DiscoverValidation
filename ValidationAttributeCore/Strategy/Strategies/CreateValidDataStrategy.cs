using DiscoverValidationCore.Helpers;
using DiscoverValidationCore.Model;
using DiscoverValidationCore.Model.Context;
using DiscoverValidationCore.Strategy.Interface;
using FluentValidation.Results;

namespace DiscoverValidationCore.Strategy.Strategies
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