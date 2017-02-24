using System;
using System.Collections.Generic;
using System.Linq;
using DiscoverValidation.GenericValidator;
using DiscoverValidation.Helpers;
using DiscoverValidation.Model;
using DiscoverValidation.Model.Context;
using DiscoverValidation.Model.Interface;
using DiscoverValidation.Strategy.Interface;
using DiscoverValidation.Strategy.Strategies;
using FluentValidation.Results;

namespace DiscoverValidation.Strategy
{
    public class ValidatorStrategyHanlder<T>
    {
        private readonly Dictionary<Func<ValidationResult, bool>, IValidatableStrategy> _strategiesCreateDataDictionary;

        public ValidatorStrategyHanlder()
        {
            _strategiesCreateDataDictionary = new Dictionary<Func<ValidationResult, bool>, IValidatableStrategy>
            {
                {vRes => vRes == null, new CreateNotValidatableDataStrategy()},
                {vRes => vRes?.IsValid == true, new CreateValidDataStrategy()},
                {vRes => vRes?.IsValid == false, new CreateInvalidDataStrategy()}
            };
        }

        public void UpdateValidationResuls(DiscoverValidatorContext context, T element)
        {
            ValidationResult validationResult = null;

            if (context.AllValidatorsDictionary.ContainsKey(element.GetType()))
            {
                validationResult = GetValidator(context, element).ValidateEntity(element);
            }

            var validatableStrategy = _strategiesCreateDataDictionary.Single(p => p.Key.Invoke(validationResult)).Value;
            validatableStrategy.UpdateValidationResuls(context, element, validationResult);
        }

        private static IDiscoverValidator GetValidator<TElement>(DiscoverValidatorContext context, TElement element)
        {
            if (context.ValidatorsInstancesDictionary.ContainsKey(element.GetType()))
            {
                return context.ValidatorsInstancesDictionary[element.GetType()];
            }

            var validatorType = context.AllValidatorsDictionary[element.GetType()];

            var validator = Activator.CreateInstance(validatorType) as IDiscoverValidator;

            RegisterValidatorInstance(context, element.GetType(), validator);

            return validator;
        }

        private static void RegisterValidatorInstance(DiscoverValidatorContext context, Type elementType, IDiscoverValidator validator)
        {
            context.ValidatorsInstancesDictionary.Add(elementType, validator);
        }

        public IData<TEntity> ValidateOneTypeEntity<TEntity>(TEntity entity, DiscoverValidatorContext context)
        {
            ValidationResult validationResult = null;

            if (context.AllValidatorsDictionary.ContainsKey(entity.GetType()))
            {
                validationResult = GetValidator(context, entity).ValidateEntity(entity);
            }

            if (validationResult == null)
            {
                return CreateInstanceFactory.CreateDataCasted(typeof(NotValidatableData<>), entity);
            }

            if (validationResult.IsValid)
            {
                return CreateInstanceFactory.CreateDataCasted(typeof(ValidData<>), entity);
            }

            var data = (InvalidData<T>)CreateInstanceFactory.CreateDataCasted(typeof(InvalidData<>), entity);
            data.ValidationFailures = validationResult.Errors;
            return (IData<TEntity>) data;
        }

    }
}