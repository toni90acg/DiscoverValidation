using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DiscoverValidation.Exceptions;
using DiscoverValidation.Extensions;
using DiscoverValidation.Helpers;
using DiscoverValidation.Model.Context;
using DiscoverValidation.Model.Data.Interface;
using DiscoverValidation.Model.ValidationResults;
using FluentValidation.Results;

namespace DiscoverValidation.Application
{
    public static class DiscoverValidator
    {
        internal static DiscoverValidatorContext DVcontext;
        public static Task InitializedTask;

        /// <summary>
        /// Initializes the DiscoverValidator
        /// </summary>
        /// <param name="assembly">Specify the assembly of the validators to make it faster</param>
        public static void Initialize(Assembly assembly = null)
        {
            DVcontext = CreateInstanceFactory.CreateDiscoverValidationContext(assembly);
        }

        /// <summary>
        /// Initializes the DiscoverValidator asynchronously
        /// </summary>
        /// <param name="assembly">Specify the assembly of the validators to make it faster</param>
        /// <returns>Returns the running Task</returns>
        public static Task InitializeAssync(Assembly assembly = null)
        {
            InitializedTask = Task.Run(()=>DVcontext = CreateInstanceFactory.CreateDiscoverValidationContext(assembly));
            return InitializedTask;
        }
       
        #region Validate Entity

        /// <summary>
        /// Validate one unique entity
        /// </summary>
        /// <typeparam name="T">Type of the entity to validate</typeparam>
        /// <param name="element">Entity to validate</param>
        /// <param name="useThisValidatorType">Optional parameter to use an specific validator. It has to implement IDiscoverValidator</param>
        /// <returns>Returns an IData of type T</returns>
        public static IData<T> ValidateEntity<T>(T element, Type useThisValidatorType = null)
        {
            IsReady();
            var validatorStrategyHandler = CreateInstanceFactory.CreateValidatorStrategyHandler<T>();
            var validator = useThisValidatorType == null
                ? validatorStrategyHandler.GetValidator(DVcontext, element)
                : validatorStrategyHandler.GetValidator(DVcontext, useThisValidatorType);

            if (validator == null) throw new DiscoverValidationCreatingValidatorException($"You need to specify a validator type for the entity of type {element.GetType().Name}");

            return validatorStrategyHandler.ValidateOneTypeEntity(element, DVcontext, validator);
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
            IsReady();
            return elements.Select(element => ValidateEntity(element, useThisValidatorType)).ToList();
        }

        /// <summary>
        /// Validate a list of entities of one unique type as Parallel
        /// </summary>
        /// <typeparam name="T">Type of the entities to validate</typeparam>
        /// <param name="elements">Entities to validate</param>
        /// <param name="useThisValidatorType">Optional parameter to use an specific validator. It has to implement IDiscoverValidator</param>
        /// <returns>Returns a list of IData of type T</returns>
        public static List<IData<T>> ValidateEntityParallel<T>(List<T> elements, Type useThisValidatorType = null)
        {
            IsReady();
            var validatorStrategyHandler = CreateInstanceFactory.CreateValidatorStrategyHandler<T>();
            var validator = useThisValidatorType == null
                ? validatorStrategyHandler.GetValidator(DVcontext, elements.First())
                : validatorStrategyHandler.GetValidator(DVcontext, useThisValidatorType);

            return elements.AsParallel()
                .Select(element => validatorStrategyHandler.ValidateOneTypeEntity(element, DVcontext, validator))
                .ToList();
        }

        #endregion

        #region Validate Multiple Entities
                
        public static DiscoverValidationResults ValidateMultipleEntities<T>(IList<T> entities)
        {
            IsReady();
            if (DVcontext?.DiscoverValidationResults == null) DVcontext = CreateInstanceFactory.InitializeDiscoverValidationResults(DVcontext);
            var validatorStrategyHandler = CreateInstanceFactory.CreateValidatorStrategyHandler<T>();

            return DVcontext.CreateValidatorInstances(entities)
                .ValidateEntities(entities, validatorStrategyHandler);
        }

        public static DiscoverValidationResults ValidateMultipleEntitiesParallel<T>(IList<T> entities)
        {
            IsReady();
            DVcontext = CreateInstanceFactory.InitializeDiscoverValidationResults(DVcontext);
            var validatorStrategyHandler = CreateInstanceFactory.CreateValidatorStrategyHandler<T>();
            
            DVcontext.CreateValidatorInstancesParallel(entities)
                .ValidateEntitiesParallel(entities, validatorStrategyHandler);

            return DVcontext.DiscoverValidationResults;
        }

        #endregion

        #region Private methods

        private static void IsReady()
        {
            InitializedTask?.Wait();
            if (DVcontext == null)
            {
                Initialize();
            }
        }

        #endregion
    }
}
