using System;
using System.Collections.Generic;
using System.Linq;
using DiscoverValidation.Exceptions;
using DiscoverValidation.Extensions;
using DiscoverValidation.GenericValidator;
using DiscoverValidation.Model.Context;
using DiscoverValidation.Model.Data.Interface;
using DiscoverValidation.Model.ValidationResults;
using DiscoverValidation.Strategy;
using FluentValidation.Results;

namespace DiscoverValidation.Helpers
{
    public static class CreateInstanceFactory
    {
        internal static IData<T> CreateDataCasted<T>(Type typeOfData, T element,
            IList<ValidationFailure> failures = null, IList<Type> validators = null)
        {
            return (IData<T>) CreateData(typeOfData, element, failures, validators);
        }

        internal static object CreateData(Type typeOfData, object element, IList<ValidationFailure> failures = null,
            IList<Type> validators = null)
        {
            Type[] typeArgs = {element.GetType()};
            var makeme = typeOfData.MakeGenericType(typeArgs);

            if (failures == null && validators == null)
            {
                try
                {
                    return Activator.CreateInstance(makeme, element);
                }
                catch (Exception exception)
                {
                    throw new DiscoverValidationCreatingDataException(
                        $"Error creating an instance of {makeme} for the entity of type {element.GetType().Name}",
                        exception);
                }
            }
            if (validators == null)
            {
                var ctorParams = new[]
                {
                    element,
                    failures
                };

                try
                {
                    return Activator.CreateInstance(makeme, ctorParams);
                }
                catch (Exception exception)
                {
                    throw new DiscoverValidationCreatingDataException(
                        $"Error creating an instance of {makeme} for the entity of type {element.GetType().Name}",
                        exception);
                }
            }
            else
            {
                var ctorParams = new[]
                {
                    element,
                    validators
                };

                try
                {
                    return Activator.CreateInstance(makeme, ctorParams);
                }
                catch (Exception exception)
                {
                    throw new DiscoverValidationCreatingDataException(
                        $"Error creating an instance of {makeme} for the entity of type {element.GetType().Name}",
                        exception);
                }
            }
        }

        internal static DiscoverValidatorContext CreateDiscoverValidationContext()
        {
            return new DiscoverValidatorContext()
                .LoadValidatorsDictionary()
                .InitializeEmptyValidatorsInstancesDictionary();
        }

        internal static DiscoverValidatorContext CreateDiscoverValidationResults(DiscoverValidatorContext context)
        {
            context.DiscoverValidationResults = new DiscoverValidationResults()
            {
                ValidatableEntityTypes = context.ValidatorsTypesDictionary.Keys.ToList()
            };

            return context;
        }

        internal static ValidatorStrategyHanlder<T> CreateValidatorStrategyHandler<T>()
        {
            return new ValidatorStrategyHanlder<T>();
        }

        internal static IDiscoverValidator CreateValidator(Type validatorType)
        {
            var validator = Activator.CreateInstance(validatorType) as IDiscoverValidator;
            if (validator == null)
            {
                throw new DiscoverValidationCreatingValidatorException($"Error creating the validator. The type {validatorType} doesn't implement the AbstractDiscoverValidator class.");
            }
            return validator;
        }
    }
}
