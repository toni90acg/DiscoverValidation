using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
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
            ValidatorsDictionary = Helper.LoadValidators();
        }
        
        public static IData<T> ValidateEntity<T>(T element)
        {
            if (!ValidatorsDictionary.ContainsKey(element.GetType()))
                return CreateData(typeof(NotValidatableData<>), element);

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
            //ToDo Apply Strategy Pattern
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

                        results.EntityTypesWithInvalidValidations.Add(element.GetType());
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

        #region Internal Methods

        internal static IData<T> CreateData<T>(Type typeOfData, T element, IList<ValidationFailure> failures = null)
        {
            return (IData<T>) CreateDataObj(typeOfData, element, failures);
        }

        internal static object CreateDataObj(Type typeOfData, object element, IList<ValidationFailure> failures = null)
        {
            Type[] typeArgs = {element.GetType()};
            var makeme = typeOfData.MakeGenericType(typeArgs);

            if (failures == null)
            {
                return Activator.CreateInstance(makeme, element);
            }
            var ctorParams = new[]
            {
                element,
                failures
            };
            return Activator.CreateInstance(makeme, ctorParams);
        }
        
        #endregion
    }
}
