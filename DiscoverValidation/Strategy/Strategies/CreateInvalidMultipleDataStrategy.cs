using System;
using System.Collections.Generic;
using DiscoverValidation.Helpers;
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
            //var data = CreateInstanceFactory.CreateData(typeof(InvalidData<>), element, validationResult.Errors);
            var data = CreateData(element, validationResult.Errors);

            //context.DiscoverValidationResults.InvalidDataList.Add(data);
            //context.DiscoverValidationResults.AllDataList.Add(data);
            UpdateData(context, data);
        }

        public void UpdateValidationResulsLock<T>(DiscoverValidatorContext context, T element, object lockObject,
            ValidationResult validationResult)
        {
            //var data = CreateInstanceFactory.CreateData(typeof(InvalidData<>), element, validationResult.Errors);
            var data = CreateData(element, validationResult.Errors);

            lock (lockObject)
            {
                //context.DiscoverValidationResults.InvalidDataList.Add(data);
                //context.DiscoverValidationResults.AllDataList.Add(data);
                UpdateData(context, data);
            }
        }

        private object CreateData(object element, IList<ValidationFailure> failures)
        {
            return CreateInstanceFactory.CreateData(typeof(InvalidData<>), element, failures);
        }

        private void UpdateData(DiscoverValidatorContext context, object data)
        {
            context.DiscoverValidationResults.InvalidDataList.Add(data);
            context.DiscoverValidationResults.AllDataList.Add(data);
        }
    }
}