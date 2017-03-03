using System;
using System.Collections.Generic;
using System.Linq;
using DiscoverValidation.Helpers;
using DiscoverValidation.Model.Context;
using DiscoverValidation.Model.Data.Interface;
using DiscoverValidation.Model.ValidationResults;

namespace DiscoverValidation.Application
{
    public static class DiscoverValidator
    {
        internal static DiscoverValidatorContext DVcontext;

        static DiscoverValidator()
        {
            DVcontext = CreateInstanceFactory.CreateDiscoverValidationContext();
        }

        /// <summary>
        /// Validate one unique entity
        /// </summary>
        /// <typeparam name="T">Type of the entity to validate</typeparam>
        /// <param name="element">Entity to validate</param>
        /// <param name="useThisValidatorType">Optional parameter to use an specific validator. It has to implement IDiscoverValidator</param>
        /// <returns>Returns an IData of type T</returns>
        public static IData<T> ValidateEntity<T>(T element, Type useThisValidatorType = null)
        {
            var validatorStrategyHandler = CreateInstanceFactory.CreateValidatorStrategyHandler<T>();
            return validatorStrategyHandler.ValidateOneTypeEntity(element, DVcontext, useThisValidatorType);
        }

        /// <summary>
        /// Validate a list of entities of one unique type
        /// </summary>
        /// <typeparam name="T">Type of the entities to validate</typeparam>
        /// <param name="elements">Entities to validate</param>
        /// <param name="useThisValidatorType">Optional parameter to use an specific validator. It has to implement IDiscoverValidator</param>
        /// <returns>Returns a list of IData of type T</returns>
        public static List<IData<T>> ValidateEntity<T>(List<T> elements, Type useThisValidatorType = null)
        {
            return elements.Select(element => ValidateEntity(element, useThisValidatorType)).ToList();
        }

        /// <summary>
        /// Validate a list of entities of any type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities">Entities to validate</param>
        /// <returns>Returns a DiscoverValidationResults with all the validations results</returns>
        public static DiscoverValidationResults ValidateMultipleEntities<T>(IList<T> entities)
        {
            DVcontext = CreateInstanceFactory.CreateDiscoverValidationResults(DVcontext);
            var validatorStrategyHandler = CreateInstanceFactory.CreateValidatorStrategyHandler<T>();

            entities.ToList().ForEach(element =>
            {
                validatorStrategyHandler.UpdateValidationResuls(DVcontext, element);
            });

            return DVcontext.DiscoverValidationResults;
        }
    }
}
