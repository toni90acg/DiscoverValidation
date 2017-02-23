using DiscoverValidationCore.Helpers;
using DiscoverValidationCore.Model;
using DiscoverValidationCore.Model.Context;
using DiscoverValidationCore.Strategy.Interface;
using FluentValidation.Results;

namespace DiscoverValidationCore.Strategy.Strategies
{
    public class CreateNotValidatableDataStrategy : IValidatableStrategy
    {
        public void UpdateValidationResuls<T>(DiscoverValidatorContext context, T element, ValidationResult validationResult = null)
        {
            var data = CreateInstanceFactory.CreateData(typeof(NotValidatableData<>), element);

            context.DiscoverValidationResults.NotValidatableEntityTypes.Add(element.GetType());
            context.DiscoverValidationResults.NotValidatableDataList.Add(data);
            context.DiscoverValidationResults.AllDataList.Add(data);
        }
    }
}