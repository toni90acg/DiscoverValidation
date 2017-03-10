using DiscoverValidation.Helpers;
using DiscoverValidation.Model;
using DiscoverValidation.Model.Context;
using DiscoverValidation.Model.Data;
using DiscoverValidation.Strategy.Interface;
using FluentValidation.Results;

namespace DiscoverValidation.Strategy.Strategies
{
    public class CreateInvalidDataStrategy : IValidatableStrategy
    {
        public void UpdateValidationResuls<T>(DiscoverValidatorContext context, T element, ValidationResult validationResult)
        {
            var data = CreateInstanceFactory.CreateData(typeof(InvalidData<>), element, validationResult.Errors);

            context.DiscoverValidationResults.EntityTypesWithInvalidValidations.Add(element.GetType());
            context.DiscoverValidationResults.InvalidDataList.Add(data);
            context.DiscoverValidationResults.AllDataList.Add(data);
        }

        public void UpdateValidationResulsLock<T>(DiscoverValidatorContext context, T element, object lockObject,
            ValidationResult validationResult)
        {
            var data = CreateInstanceFactory.CreateData(typeof(InvalidData<>), element, validationResult.Errors);
            
            lock (lockObject)
            {
                context.DiscoverValidationResults.EntityTypesWithInvalidValidations.Add(element.GetType());
                context.DiscoverValidationResults.InvalidDataList.Add(data);
                context.DiscoverValidationResults.AllDataList.Add(data);
            }
        }
    }
}