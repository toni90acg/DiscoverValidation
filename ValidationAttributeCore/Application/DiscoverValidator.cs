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
            ValidatorsDictionary = AssembliesHelper.LoadValidators();
        }
        
        public static IData<T> ValidateEntity<T>(T element)
        {
            if (!ValidatorsDictionary.ContainsKey(element.GetType()))
                return CreateInstanceHelper.CreateDataCasted(typeof(NotValidatableData<>), element);

            var validatorType = ValidatorsDictionary[element.GetType()];

            var validator = (IDiscoverValidator) Activator.CreateInstance(validatorType);
            var results = validator.ValidateEntity(element);

            if (results.IsValid)
            {
                return CreateInstanceHelper.CreateDataCasted(typeof(ValidData<>), element);
            }
            else
            {
                var data = (InvalidData<T>)CreateInstanceHelper.CreateDataCasted(typeof(InvalidData<>), element);
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

                    var validator = (IDiscoverValidator)Activator.CreateInstance(validatorType);
                    var validationResult = validator.ValidateEntity(element);

                    if (validationResult == null)
                    {
                        var data = CreateInstanceHelper.CreateData(typeof(NotValidatableData<>), element);
                        results.NotValidatableEntityTypes.Add(element.GetType());
                        results.NotValidatableDataList.Add(data);
                        results.AllDataList.Add(data);
                    }

                    else if (validationResult.IsValid)
                    {
                        var data = CreateInstanceHelper.CreateData(typeof(ValidData<>), element);
                        results.ValidDataList.Add(data);
                        results.AllDataList.Add(data);
                    }
                    else
                    {
                        var data = CreateInstanceHelper.CreateData(typeof(InvalidData<>), element, validationResult.Errors);

                        results.EntityTypesWithInvalidValidations.Add(element.GetType());
                        results.InvalidDataList.Add(data);
                        results.AllDataList.Add(data);
                    }
                }
                else
                {
                    var data = CreateInstanceHelper.CreateData(typeof(NotValidatableData<>), element);
                    results.NotValidatableEntityTypes.Add(element.GetType());
                    results.NotValidatableDataList.Add(data);
                    results.AllDataList.Add(data);
                }
            }

            return results;
        }
    }
}
