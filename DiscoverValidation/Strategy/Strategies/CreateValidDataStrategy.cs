using System.Linq;
using DiscoverValidation.Helpers;
using DiscoverValidation.Model;
using DiscoverValidation.Model.Context;
using DiscoverValidation.Model.Data;
using DiscoverValidation.Strategy.Interface;
using FluentValidation.Results;

namespace DiscoverValidation.Strategy.Strategies
{
    public class CreateValidDataStrategy : IValidatableStrategy
    {
        public void UpdateValidationResuls<T>(DiscoverValidatorContext context, T element, ValidationResult validationResult = null)
        {
            //var data = CreateInstanceFactory.CreateData(typeof(ValidData<>), element);
            var data = CreateData(element);

            UpdateData(context, element, data);
            //context.DiscoverValidationResults.ValidDataList.Add(data);
            //context.DiscoverValidationResults.AllDataList.Add(data);
        }

        public void UpdateValidationResulsLock<T>(DiscoverValidatorContext context, T element, object lockObject,
            ValidationResult validationResult = null)
        {
            //var data = CreateInstanceFactory.CreateData(typeof(ValidData<>), element);
            var data = CreateData(element);

            lock (lockObject)
            {
                UpdateData(context, element, data);
                //context.DiscoverValidationResults.ValidDataList.Add(data);
                //context.DiscoverValidationResults.AllDataList.Add(data);
            }
        }

        private object CreateData(object element)
        {
            return CreateInstanceFactory.CreateData(typeof(ValidData<>), element);
        }

        private void UpdateData<T>(DiscoverValidatorContext context, T element, object data)
        {
            context.DiscoverValidationResults.ValidDataList.Add(data);
            context.DiscoverValidationResults.AllDataList.Add(data);
        }
    }
}