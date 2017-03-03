using DiscoverValidation.Helpers;
using DiscoverValidation.Model;
using DiscoverValidation.Model.Context;
using DiscoverValidation.Model.Data;
using DiscoverValidation.Strategy.Interface;
using FluentValidation.Results;

namespace DiscoverValidation.Strategy.Strategies
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