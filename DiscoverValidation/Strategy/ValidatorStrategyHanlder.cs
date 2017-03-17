using System;
using System.Collections.Generic;
using System.Linq;
using DiscoverValidation.Exceptions;
using DiscoverValidation.Extensions;
using DiscoverValidation.GenericValidator;
using DiscoverValidation.Helpers;
using DiscoverValidation.Model.Context;
using DiscoverValidation.Model.Data;
using DiscoverValidation.Model.Data.Interface;
using DiscoverValidation.Strategy.Interface;
using DiscoverValidation.Strategy.Strategies;
using FluentValidation.Results;

namespace DiscoverValidation.Strategy
{
    public class ValidatorStrategyHanlder<T>
    {
        private readonly object _thisLock = new object();
        private readonly Dictionary<Func<DiscoverValidatorContext, T , ValidationResult, bool>, IValidatableStrategy> _strategiesCreateDataDictionary;

        public ValidatorStrategyHanlder()
        {
            _strategiesCreateDataDictionary = new Dictionary
                <Func<DiscoverValidatorContext, T, ValidationResult, bool>, IValidatableStrategy>
            {
                {
                    (context, element, vR) =>
                        vR == null &&
                        context.EntitiesWithMultiplesValidators.Any(ewmv => ewmv.EntityType != element.GetType()),
                    new CreateNotValidatableDataStrategy()
                },
                {
                    (context, element, vR) =>
                        vR == null &&
                        context.EntitiesWithMultiplesValidators.Any(ewmv => ewmv.EntityType == element.GetType()),
                    new CreateNotValidatedDataStrategy()
                },
                {(context, element, vR) => vR?.IsValid == true, new CreateValidDataStrategy()},
                {(context, element, vR) => vR?.IsValid == false, new CreateInvalidDataStrategy()}
            };
        }

        public IData<TEntity> ValidateOneTypeEntity<TEntity>(TEntity entity, DiscoverValidatorContext context, IDiscoverValidator validator)
        {
            var validationResult = validator.ValidateEntity(entity);

            if (validationResult == null)
            {
                var entitiesWithMultiplesValidators =
                    context.EntitiesWithMultiplesValidators.SingleOrDefault(ewmv => ewmv.EntityType == entity.GetType());
                if (entitiesWithMultiplesValidators != null)
                {
                    return CreateInstanceFactory.CreateDataCasted(typeof(NotValidatedData<>), entity, validators: entitiesWithMultiplesValidators.Validators);
                }
                return CreateInstanceFactory.CreateDataCasted(typeof(NotValidatableData<>), entity);
            }

            if (validationResult.IsValid)
            {
                return CreateInstanceFactory.CreateDataCasted(typeof(ValidData<>), entity);
            }

            var data = (InvalidData<T>)CreateInstanceFactory.CreateDataCasted(typeof(InvalidData<>), entity);
            data.ValidationFailures = validationResult.Errors;
            return (IData<TEntity>)data;
        }

        #region UpdateValidationResults

        public void UpdateValidationResults(DiscoverValidatorContext context, T element, ValidationResult validationResult)
        {
            var validatableStrategy = _strategiesCreateDataDictionary.Single(p => p.Key.Invoke(context, element, validationResult)).Value;
            validatableStrategy.UpdateValidationResuls(context, element, validationResult);
        }

        public void UpdateValidationResultsLock(DiscoverValidatorContext context, T element, ValidationResult validationResult)
        {
            var validatableStrategy = _strategiesCreateDataDictionary.Single(p => p.Key.Invoke(context, element, validationResult)).Value;
            validatableStrategy.UpdateValidationResulsLock(context, element, _thisLock, validationResult);
        }

        #endregion

        #region GetValidator

        internal IDiscoverValidator GetValidator<TElement>(DiscoverValidatorContext context, TElement element)
        {
            if (context.ValidatorsInstancesDictionary.ContainsKey(element.GetType()))
            {
                return context.ValidatorsInstancesDictionary[element.GetType()];
            }

            if (context.ValidatorsTypesDictionary.ContainsKey(element.GetType()))
            {
                var validatorType = context.ValidatorsTypesDictionary[element.GetType()];
                var validator = CreateInstanceFactory.CreateValidator(validatorType);
                context.RegisterValidatorInstance(element.GetType(), validator);
                return validator;
            }

            return null;
        }

        internal IDiscoverValidator GetValidatorLock<TElement>(DiscoverValidatorContext context, TElement element)
        {
            lock (_thisLock)
            {
                if (context.ValidatorsInstancesDictionary.ContainsKey(element.GetType()))
                {
                    return context.ValidatorsInstancesDictionary[element.GetType()];
                }

                if (context.ValidatorsTypesDictionary.ContainsKey(element.GetType()))
                {
                    var validatorType = context.ValidatorsTypesDictionary[element.GetType()];
                    var validator = CreateInstanceFactory.CreateValidator(validatorType);
                    context.RegisterValidatorInstance(element.GetType(), validator);
                    return validator;
                }
            }

            return null;
        }

        internal IDiscoverValidator GetValidator(DiscoverValidatorContext context, Type useThisValidatorType)
        {
            var validator = context
                .ValidatorsAlternativeInstances
                .SingleOrDefault(pair => pair.Left == useThisValidatorType)?
                .Rigth;

            if (validator != null)
            {
                return validator;
            }

            if (AssembliesHelper.IsAssignableToGenericType(useThisValidatorType, typeof(AbstractDiscoverValidator<>)))
            {
                validator = CreateInstanceFactory.CreateValidator(useThisValidatorType);

                context.ValidatorsAlternativeInstances.Add(new Pair<Type, IDiscoverValidator>(useThisValidatorType, validator));

                return validator;
            }

            throw new DiscoverValidationCreatingValidatorException($"The specified validator type ({useThisValidatorType.Name}) is not valid, you have to use a validator that inherits from AbstractDiscoverValidator.");
        }

        #endregion
    }
}