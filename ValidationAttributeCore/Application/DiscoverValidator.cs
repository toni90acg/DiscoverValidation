using System;
using System.Collections.Generic;
using System.Linq;
using DiscoverValidationCore.GenericValidator;
using DiscoverValidationCore.Helpers;
using DiscoverValidationCore.Model;
using DiscoverValidationCore.Model.Context;
using DiscoverValidationCore.Model.Interface;
using DiscoverValidationCore.Model.ValidationResults;

namespace DiscoverValidationCore.Application
{
    public static class DiscoverValidator
    {
        internal static DiscoverValidatorContext Context;

        static DiscoverValidator()
        {
            Context = CreateInstanceFactory.CreateDiscoverValidationContext();
        }
        
        /// <summary>
        /// Validate one unique entity
        /// </summary>
        /// <typeparam name="T">Type of the entity to validate</typeparam>
        /// <param name="element">Entity to validate</param>
        /// <returns>Returns an IData of type T</returns>
        public static IData<T> ValidateEntity<T>(T element)
        {
            if (!Context.AllValidatorsDictionary.ContainsKey(element.GetType()))
                return CreateInstanceFactory.CreateDataCasted(typeof(NotValidatableData<>), element);

            var validatorType = Context.AllValidatorsDictionary[element.GetType()];

            var validator = (IDiscoverValidator) Activator.CreateInstance(validatorType);
            var results = validator.ValidateEntity(element);

            if (results.IsValid)
            {
                return CreateInstanceFactory.CreateDataCasted(typeof(ValidData<>), element);
            }
            else
            {
                var data = (InvalidData<T>)CreateInstanceFactory.CreateDataCasted(typeof(InvalidData<>), element);
                data.ValidationFailures = results.Errors;
                return data;
            }
        }

        /// <summary>
        /// Validate a list of entities of one unique type
        /// </summary>
        /// <typeparam name="T">Type of the entities to validate</typeparam>
        /// <param name="elements">Entities to validate</param>
        /// <returns>Returns a list of IData of type T</returns>
        public static List<IData<T>> ValidateEntity<T>(List<T> elements)
        {
            var result = new List<IData<T>>();
            foreach (var element in elements)
            {
                result.Add(ValidateEntity(element));
            }
            return result;
        }

        /// <summary>
        /// Validate a list of entities of any type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities">Entities to validate</param>
        /// <returns>Returns a DiscoverValidationResults with all the validations results</returns>
        public static DiscoverValidationResults ValidateMultipleEntities<T>(IList<T> entities)
        {
            Context = CreateInstanceFactory.CreateDiscoverValidationResults(Context);
            var validatorStrategyHandler = CreateInstanceFactory.CreateValidatorStrategyHandler<T>();

            entities.ToList().ForEach(element =>
            {
                validatorStrategyHandler.UpdateValidationResuls(Context, element);
            });

            return Context.DiscoverValidationResults;
        }
    }
}
