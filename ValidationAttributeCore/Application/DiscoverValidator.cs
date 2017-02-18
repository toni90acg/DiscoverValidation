using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentValidation.Results;
using ValidationAttributeCore.CustomAttribute;
using ValidationAttributeCore.GenericValidator;
using ValidationAttributeCore.Helpers;
using ValidationAttributeCore.Model;
using ValidationAttributeCore.Model.Interface;
using ValidationAttributeCore.Model.ValidationResults;

namespace ValidationAttributeCore.Application
{
    public static class DiscoverValidator
    {
        internal static Dictionary<Type, Type> ValidatorsDictionary;

        static DiscoverValidator()
        {
            ValidatorsDictionary = new Dictionary<Type, Type>();
            LoadValidators();
        }
        
        public static IData<T> ValidateEntity<T>(T element)
        {
            if (ValidatorsDictionary.ContainsKey(element.GetType()))
            {
                var validatorType = ValidatorsDictionary[element.GetType()];

                var validator = (IAttributeValidator) Activator.CreateInstance(validatorType);
                var results = validator.ValidateEntity(element);

                if (results.IsValid)
                {
                    return CreateData(typeof(ValidData<>), element);
                }
                else
                {
                    var data = (InvalidData<T>) CreateData(typeof(InvalidData<>), element);
                    data.ValidationFailures = results.Errors;
                    return data;
                }
            }
            return CreateData(typeof(NotValidatableData<>), element);
        }

        public static List<IData<T>> ValidateEntity<T>(List<T> elements)
        {
            var result = new List<IData<T>>();
            foreach (var element in elements)
            {
                result.Add(ValidateEntity(element));
            }
            return result;
        }

        public static DiscoverValidationResults ValidateMultipleEntities<T>(IList<T> coleccion)
        {
            var results = new DiscoverValidationResults { ValidatableEntityTypes = ValidatorsDictionary.Keys.ToList() };

            foreach (var element in coleccion)
            {
                if (ValidatorsDictionary.ContainsKey(element.GetType()))
                {
                    var validatorType = ValidatorsDictionary[element.GetType()];

                    var validator = (IAttributeValidator)Activator.CreateInstance(validatorType);
                    var validationResult = validator.ValidateEntity(element);

                    if (validationResult == null)
                    {
                        var data = CreateDataObj(typeof(NotValidatableData<>), element);
                        results.NotValidatableEntityTypes.Add(element.GetType());
                        results.NotValidatableDataList.Add(data);
                        results.AllDataList.Add(data);
                    }

                    else if (validationResult.IsValid)
                    {
                        var data = CreateDataObj(typeof(ValidData<>), element);
                        results.ValidDataList.Add(data);
                        results.AllDataList.Add(data);
                    }
                    else
                    {
                        var data = CreateDataObj(typeof(InvalidData<>), element, validationResult.Errors);

                        results.InvalidDataList.Add(data);
                        results.AllDataList.Add(data);
                    }
                }
                else
                {
                    var data = CreateDataObj(typeof(NotValidatableData<>), element);
                    results.NotValidatableEntityTypes.Add(element.GetType());
                    results.NotValidatableDataList.Add(data);
                    results.AllDataList.Add(data);
                }
            }

            return results;
        }

        #region Private Methods

        internal static IData<T> CreateData<T>(Type typeOfData, T element)
        {
            return (IData<T>) CreateDataObj(typeOfData, element);
        }

        internal static object CreateDataObj(Type typeOfData, object element)
        {
            Type[] typeArgs = {element.GetType()};
            var makeme = typeOfData.MakeGenericType(typeArgs);
            return Activator.CreateInstance(makeme, element);
        }

        internal static object CreateDataObj(Type typeOfData, object element, IList<ValidationFailure> failures)
        {
            Type[] typeArgs = { element.GetType() };
            var makeme = typeOfData.MakeGenericType(typeArgs);
            var ctorParams = new[]
            {
                element,
                failures
            };
            return Activator.CreateInstance(makeme, ctorParams);
        }

        private static void LoadValidators()
        {
            AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(t => Helper.IsAssignableToGenericType(t, typeof(AbstractAttributeValidator<>)))
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .Select(s => new
                {
                    attribute = s.GetCustomAttributes<ValidateEntityAttribute>().Single(),
                    validator = s
                })
                .Where(e => e.attribute != null).ToList()
                .ForEach(e => ValidatorsDictionary.Add(e.attribute.Entity, e.validator));
        }

        #endregion
    }
}
