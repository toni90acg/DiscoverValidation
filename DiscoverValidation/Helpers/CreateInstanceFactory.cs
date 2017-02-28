using System;
using System.Collections.Generic;
using System.Linq;
using DiscoverValidation.GenericValidator;
using DiscoverValidation.Model.Context;
using DiscoverValidation.Model.Interface;
using DiscoverValidation.Model.ValidationResults;
using DiscoverValidation.Strategy;
using FluentValidation.Results;

namespace DiscoverValidation.Helpers
{
    public static class CreateInstanceFactory
    {
        internal static IData<T> CreateDataCasted<T>(Type typeOfData, T element, IList<ValidationFailure> failures = null)
        {
            return (IData<T>)CreateData(typeOfData, element, failures);
        }

        internal static object CreateData(Type typeOfData, object element, IList<ValidationFailure> failures = null)
        {
            Type[] typeArgs = { element.GetType() };
            var makeme = typeOfData.MakeGenericType(typeArgs);

            if (failures == null)
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
            var ctorParams = new[]
            {
                element,
                failures
            };
            return Activator.CreateInstance(makeme, ctorParams);
        }

        internal static DiscoverValidatorContext CreateDiscoverValidationContext()
        {
            var context = new DiscoverValidatorContext
            {
                AllValidatorsDictionary = AssembliesHelper.LoadValidators(),
                ValidatorsInstancesDictionary = new Dictionary<Type, IDiscoverValidator>()
            };
            return context;
        }

        internal static DiscoverValidatorContext CreateDiscoverValidationResults(DiscoverValidatorContext context)
        {
            context.DiscoverValidationResults = new DiscoverValidationResults()
            {
                ValidatableEntityTypes = context.AllValidatorsDictionary.Keys.ToList()
            };

            return context;
        }

        internal static ValidatorStrategyHanlder<T> CreateValidatorStrategyHandler<T>()
        {
            return new ValidatorStrategyHanlder<T>();
        }

    }

    internal class DiscoverValidationCreatingDataException : Exception
    {
        public DiscoverValidationCreatingDataException(string message,
            Exception innerException) : base(message, innerException)
        {
        }
    }
}
