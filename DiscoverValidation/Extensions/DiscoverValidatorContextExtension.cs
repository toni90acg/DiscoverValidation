using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DiscoverValidation.GenericValidator;
using DiscoverValidation.Helpers;
using DiscoverValidation.Model.Context;
using DiscoverValidation.Model.ValidationResults;
using DiscoverValidation.Strategy;
using FluentValidation.Results;

namespace DiscoverValidation.Extensions
{
    internal static class DiscoverValidatorContextExtension
    {
        internal static DiscoverValidatorContext LoadValidatorsDictionary(this DiscoverValidatorContext context, Assembly assembly)
        {
            List<EntityWithMultipleValidators> entitiesWithMultipleValidators;
            context.ValidatorsTypesDictionary = AssembliesHelper.LoadValidators(assembly, out entitiesWithMultipleValidators);
            context.EntitiesWithMultiplesValidators = entitiesWithMultipleValidators;
            return context;
        }

        internal static DiscoverValidatorContext InitializeEmptyValidatorsInstancesDictionary(this DiscoverValidatorContext context)
        {
            context.ValidatorsInstancesDictionary = new Dictionary<Type, IDiscoverValidator>();
            context.ValidatorsAlternativeInstances = new List<Pair<Type, IDiscoverValidator>>();
            return context;
        }

        #region ValidatorInstances

        internal static IDiscoverValidator GetValidator<TElement>(this DiscoverValidatorContext context, TElement element)
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

        internal static DiscoverValidatorContext CreateValidatorInstances<T>(this DiscoverValidatorContext context,
            IList<T> entities)
        {
            context.ValidatorsTypesDictionary
                .Where(d => entities
                    .Select(e => e.GetType())
                    .Distinct().Contains(d.Key))
                .ForEach(d =>
                {
                    if (!context.ValidatorsInstancesDictionary.ContainsKey(d.Key))
                        context.RegisterValidatorInstance(d.Key, CreateInstanceFactory.CreateValidator(d.Value));
                });
            return context;
        }

        internal static DiscoverValidatorContext CreateValidatorInstancesParallel<T>(
            this DiscoverValidatorContext context, ICollection<T> entities)
        {
            context.ValidatorsTypesDictionary
                .Where(d => entities
                    .Select(e => e.GetType())
                    .Distinct().Contains(d.Key))
                .AsParallel()
                .ForEach(d =>
                {
                    if (!context.ValidatorsInstancesDictionary.ContainsKey(d.Key))
                        context.RegisterValidatorInstance(d.Key, CreateInstanceFactory.CreateValidator(d.Value));
                });

            return context;
        }

        internal static DiscoverValidatorContext RegisterValidatorInstance(this DiscoverValidatorContext context, Type elementType, IDiscoverValidator validator)
        {
            context.ValidatorsInstancesDictionary.Add(elementType, validator);
            return context;
        }

        #endregion

        #region ValidateEntities

        internal static DiscoverValidatorContext ValidateEntitiesParallel<T>(this DiscoverValidatorContext context,
            ICollection<T> entities, ValidatorStrategyHanlder<T> validatorStrategyHandler)
        {
            entities.AsParallel()
                .ForEach(entity =>
                    ValidateEntityAction(context, validatorStrategyHandler, entity)
                );

            return context;
        }

        internal static DiscoverValidationResults ValidateEntities<T>(this DiscoverValidatorContext context, ICollection<T> entities, ValidatorStrategyHanlder<T> validatorStrategyHandler)
        {
            entities.ForEach(entity =>
                ValidateEntityAction(context, validatorStrategyHandler, entity)
            );

            return context.DiscoverValidationResults;
        }

        private static void ValidateEntityAction<T>(DiscoverValidatorContext context, ValidatorStrategyHanlder<T> validatorStrategyHandler, T entity)
        {
            ValidationResult validationResult = null;
            if (context.ValidatorsInstancesDictionary.ContainsKey(entity.GetType()))
            {
                validationResult =
                    context.ValidatorsInstancesDictionary[entity.GetType()]
                        .ValidateEntity(entity);
            }
            validatorStrategyHandler
                .UpdateValidationResults(context, entity, validationResult);
        }

        #endregion

    }
}