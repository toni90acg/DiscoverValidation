using System.Linq;
using DiscoverValidation.Helpers;
using DiscoverValidation.Model.Context;
using DiscoverValidation.Model.Data;
using DiscoverValidation.Strategy.Interface;
using FluentValidation.Results;

namespace DiscoverValidation.Strategy.Strategies
{
    public class CreateNotValidatedDataStrategy : IValidatableStrategy
    {
        public void UpdateValidationResuls<T>(DiscoverValidatorContext context, T element, ValidationResult validationResult = null)
        {
            var validatorsFound =
                context.EntitiesWithMultiplesValidators
                .Single(ewmv => ewmv.EntityType == element.GetType())
                .Validators;
            var data = CreateInstanceFactory.CreateData(typeof(NotValidatedData<>), element, validators: validatorsFound);

            context.DiscoverValidationResults.NotValidatedEntityTypes.Add(element.GetType());
            context.DiscoverValidationResults.NotValidatedDataList.Add(data);
            context.DiscoverValidationResults.AllDataList.Add(data);
        }

        public void UpdateValidationResulsLock<T>(DiscoverValidatorContext context, T element, object lockObject,
            ValidationResult validationResult = null)
        {
            var validatorsFound =
                context.EntitiesWithMultiplesValidators
                    .Single(ewmv => ewmv.EntityType == element.GetType())
                    .Validators;
            var data = CreateInstanceFactory.CreateData(typeof(NotValidatedData<>), element, validators: validatorsFound);
            
            lock (lockObject)
            {
                context.DiscoverValidationResults.NotValidatedEntityTypes.Add(element.GetType());
                context.DiscoverValidationResults.NotValidatedDataList.Add(data);
                context.DiscoverValidationResults.AllDataList.Add(data);
            }
        }
    }
}