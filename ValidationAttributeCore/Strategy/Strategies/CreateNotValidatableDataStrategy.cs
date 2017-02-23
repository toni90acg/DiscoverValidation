using FluentValidation.Results;
using ValidationAttributeCore.Helpers;
using ValidationAttributeCore.Model;
using ValidationAttributeCore.Model.Context;
using ValidationAttributeCore.Strategy.Interface;

namespace ValidationAttributeCore.Strategy.Strategies
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