using System;
using System.Collections.Generic;
using System.Linq;
using DiscoverValidation.Extensions;
using DiscoverValidation.GenericValidator;
using DiscoverValidation.Helpers;
using DiscoverValidation.Model;
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
        private readonly Dictionary<Func<DiscoverValidatorContext, T , ValidationResult, bool>, IValidatableStrategy> _strategiesCreateDataDictionary;

        public ValidatorStrategyHanlder()
        {
            _strategiesCreateDataDictionary = new Dictionary
                <Func<DiscoverValidatorContext, T, ValidationResult, bool>, IValidatableStrategy>
            {
                {
                    (context, element, vR) =>
                        context.GetValidator(element) == null &&
                        context.EntitiesWithMultiplesValidators.Any(ewmv => ewmv.EntityType != element.GetType()),
                    new CreateNotValidatableDataStrategy()
                },
                {
                    (context, element, vR) =>
                        context.GetValidator(element) == null &&
                        context.EntitiesWithMultiplesValidators.Any(ewmv => ewmv.EntityType == element.GetType()),
                    new CreateNotValidatedDataStrategy()
                },
                {(context, element, vR) => vR?.IsValid == true, new CreateValidDataStrategy()},
                {(context, element, vR) => vR?.IsValid == false, new CreateInvalidDataStrategy()}
            };
        }

        public void UpdateValidationResuls(DiscoverValidatorContext context, T element)
        {
            ValidationResult validationResult = null;

            if (!context.ValidatorsTypesDictionary.ContainsKey(element.GetType()))
            {

                var entitiWithMultiplesValidators = context.EntitiesWithMultiplesValidators
                    .SingleOrDefault(ewmv => ewmv.EntityType == element.GetType());

                if(entitiWithMultiplesValidators == null)
                {
                    //ToDo Usar excepcion propia
                    //throw new Exception("no tiene validador");
                }
            }
            validationResult = GetValidator(context, element)?.ValidateEntity(element);

            var validatableStrategy = _strategiesCreateDataDictionary.Single(p => p.Key.Invoke(context, element, validationResult)).Value;
            validatableStrategy.UpdateValidationResuls(context, element, validationResult);
        }

        private static IDiscoverValidator GetValidator<TElement>(DiscoverValidatorContext context, TElement element)
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

        private static IDiscoverValidator GetValidator(DiscoverValidatorContext context, Type useThisValidatorType)
        {
            var validator = context
                .ValidatorsAlternativeInstances
                .SingleOrDefault(pair => pair.Left == useThisValidatorType)?
                .Rigth;

            if (validator != null)
            {
                return validator;
            }

            validator = CreateInstanceFactory.CreateValidator(useThisValidatorType);

            context.ValidatorsAlternativeInstances.Add(new Pair<Type, IDiscoverValidator>(useThisValidatorType, validator));

            return validator;
        }

        public IData<TEntity> ValidateOneTypeEntity<TEntity>(TEntity entity, DiscoverValidatorContext context, Type useThisValidatorType)
        {
            ValidationResult validationResult = null;

            if (useThisValidatorType != null)
            {
                validationResult = GetValidator(context, useThisValidatorType).ValidateEntity(entity);
            }

            else if (context.ValidatorsTypesDictionary.ContainsKey(entity.GetType()))
            {
                validationResult = GetValidator(context, entity).ValidateEntity(entity);
            }

            if (validationResult == null)
            {
                var entitiesWithMultiplesValidators =
                    context.EntitiesWithMultiplesValidators.Single(ewmv => ewmv.EntityType == entity.GetType());
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

            var data = (InvalidData<T>) CreateInstanceFactory.CreateDataCasted(typeof(InvalidData<>), entity);
            data.ValidationFailures = validationResult.Errors;
            return (IData<TEntity>) data;
        }
    }
}