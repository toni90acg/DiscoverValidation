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
            var data = CreateData(element);
            UpdateData(context, element, data);
        }

        public void UpdateValidationResulsLock<T>(DiscoverValidatorContext context, T element, object lockObject,
            ValidationResult validationResult = null)
        {
            var data = CreateData(element);

            lock (lockObject)
            {
                UpdateData(context, element, data);
            }
        }

        private object CreateData(object element)
        {
            return CreateInstanceFactory.CreateData(typeof(NotValidatableData<>), element);
        }

        private void UpdateData<T>(DiscoverValidatorContext context, T element, object data)
        {
            context.DiscoverValidationResults.NotValidatableDataList.Add(data);
            context.DiscoverValidationResults.AllDataList.Add(data);
            if (!context.DiscoverValidationResults.NotValidatableEntityTypes.Contains(element.GetType()))
                context.DiscoverValidationResults.NotValidatableEntityTypes.Add(element.GetType());
        }
    }
}