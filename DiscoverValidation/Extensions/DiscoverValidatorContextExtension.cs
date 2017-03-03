using System;
using System.Collections.Generic;
using DiscoverValidation.GenericValidator;
using DiscoverValidation.Helpers;
using DiscoverValidation.Model.Context;
using DiscoverValidation.Model.ValidationResults;

namespace DiscoverValidation.Extensions
{
    internal static class DiscoverValidatorContextExtension
    {
        internal static DiscoverValidatorContext LoadValidatorsDictionary(this DiscoverValidatorContext context)
        {
            List<EntityWithMultipleValidators> entitiesWithMultipleValidators;
            context.ValidatorsTypesDictionary = AssembliesHelper.LoadValidators(out entitiesWithMultipleValidators);
            context.EntitiesWithMultiplesValidators = entitiesWithMultipleValidators;
            return context;
        }

        internal static DiscoverValidatorContext InitializeEmptyValidatorsInstancesDictionary(this DiscoverValidatorContext context)
        {
            context.ValidatorsInstancesDictionary = new Dictionary<Type, IDiscoverValidator>();
            return context;
        }

        internal static DiscoverValidatorContext RegisterValidatorInstance(this DiscoverValidatorContext context, Type elementType, IDiscoverValidator validator)
        {
            context.ValidatorsInstancesDictionary.Add(elementType, validator);
            return context;
        }

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
    }

    internal static class DiscoverValidatorExtension
    {
        internal static EntityWithMultipleValidators AddValidatorType(
            this EntityWithMultipleValidators entityWithMultipleValidators, Type validatorType)
        {
            entityWithMultipleValidators.Validators.Add(validatorType);
            return entityWithMultipleValidators;
        }
    }
}