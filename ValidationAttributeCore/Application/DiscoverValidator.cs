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
        internal static Dictionary<Type, IDiscoverValidator> ValidatorsInstancesDictionary;

        static DiscoverValidator()
        {
            ValidatorsDictionary = AssembliesHelper.LoadValidators();
            ValidatorsInstancesDictionary = new Dictionary<Type, IDiscoverValidator>();
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
            //ToDo Apply Strategy Pattern In progress
            var results = new DiscoverValidationResults { ValidatableEntityTypes = ValidatorsDictionary.Keys.ToList() };

            //var strategyHandler = new StrategyHanlder(results.ValidatableEntityTypes);

            foreach (var element in coleccion)
            {
                if (ValidatorsDictionary.ContainsKey(element.GetType()))
                {
                    var validatorType = ValidatorsDictionary[element.GetType()];

                    IDiscoverValidator validator;
                    
                    if (ValidatorsInstancesDictionary.ContainsKey(element.GetType()))
                    {
                        validator = ValidatorsInstancesDictionary[element.GetType()];
                    }
                    else
                    {
                        
                        validator = (IDiscoverValidator)Activator.CreateInstance(validatorType);
                        ValidatorsInstancesDictionary.Add(element.GetType(), validator);
                    }

                    
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

    public class StrategyHanlder
    {
        Dictionary<Func<Type,bool>, IValidatableStrategy> strategiesValidatablesDictionary = new Dictionary<Func<Type, bool>, IValidatableStrategy>();
        Dictionary<Func<ValidationResult, bool>, IValidatableStrategy> strategiesCreateDataDictionary = new Dictionary<Func<ValidationResult, bool>, IValidatableStrategy>();
        public StrategyHanlder(IList<Type> validatableEntityTypes)
        {
            strategiesCreateDataDictionary = new Dictionary<Func<ValidationResult, bool>, IValidatableStrategy>
            {
                {validationResult => validationResult == null, new CreateNotValidatableDataStrategy()},
                {validationResult => validationResult?.IsValid == true, new CreateValidDataStrategy()},
                {validationResult => validationResult?.IsValid == false, new CreateInvalidDataStrategy()}
            };

            strategiesValidatablesDictionary = new Dictionary<Func<Type, bool>, IValidatableStrategy>();
            strategiesValidatablesDictionary.Add(entityType => validatableEntityTypes.Contains(entityType), new CreateInvalidDataStrategy());
        }
    }

    public class CreateInvalidDataStrategy : IValidatableStrategy
    {
    }

    public class CreateValidDataStrategy : IValidatableStrategy
    {
    }

    public class CreateNotValidatableDataStrategy : IValidatableStrategy
    {
    }

    internal interface IValidatableStrategy
    {
    }
}
