using DiscoverValidationPortable.Helpers;
using DiscoverValidationPortable.Model;
using DiscoverValidationPortable.Model.Context;
using DiscoverValidationPortable.Strategy.Interface;
using FluentValidation.Results;

namespace DiscoverValidationPortable.Strategy.Strategies
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