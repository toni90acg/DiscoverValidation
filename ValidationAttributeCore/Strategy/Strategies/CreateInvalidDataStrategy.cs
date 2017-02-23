using DiscoverValidationCore.Helpers;
using DiscoverValidationCore.Model;
using DiscoverValidationCore.Model.Context;
using DiscoverValidationCore.Strategy.Interface;
using FluentValidation.Results;

namespace DiscoverValidationCore.Strategy.Strategies
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