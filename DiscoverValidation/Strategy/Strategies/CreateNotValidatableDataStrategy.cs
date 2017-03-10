using DiscoverValidation.Helpers;
using DiscoverValidation.Model.Context;
using DiscoverValidation.Model.Data;
using DiscoverValidation.Strategy.Interface;
using FluentValidation.Results;

namespace DiscoverValidation.Strategy.Strategies
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

        public void UpdateValidationResulsLock<T>(DiscoverValidatorContext context, T element, object lockObject,
            ValidationResult validationResult = null)
        {
            var data = CreateInstanceFactory.CreateData(typeof(NotValidatableData<>), element);

            lock (lockObject)
            {
                context.DiscoverValidationResults.NotValidatableEntityTypes.Add(element.GetType());
                context.DiscoverValidationResults.NotValidatableDataList.Add(data);
                context.DiscoverValidationResults.AllDataList.Add(data);
            }
        }
    }
}