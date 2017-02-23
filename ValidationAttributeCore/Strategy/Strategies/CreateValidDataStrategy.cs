using FluentValidation.Results;
using ValidationAttributeCore.Helpers;
using ValidationAttributeCore.Model;
using ValidationAttributeCore.Model.Context;
using ValidationAttributeCore.Strategy.Interface;

namespace ValidationAttributeCore.Strategy.Strategies
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