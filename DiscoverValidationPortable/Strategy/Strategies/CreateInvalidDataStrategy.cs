using DiscoverValidationPortable.Helpers;
using DiscoverValidationPortable.Model;
using DiscoverValidationPortable.Model.Context;
using DiscoverValidationPortable.Strategy.Interface;
using FluentValidation.Results;

namespace DiscoverValidationPortable.Strategy.Strategies
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
    }
}