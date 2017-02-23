using FluentValidation.Results;
using ValidationAttributeCore.Helpers;
using ValidationAttributeCore.Model;
using ValidationAttributeCore.Model.Context;
using ValidationAttributeCore.Strategy.Interface;

namespace ValidationAttributeCore.Strategy.Strategies
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