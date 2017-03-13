using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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

        public static void Initialize(Assembly assembly = null)
        {
            DVcontext = CreateInstanceFactory.CreateDiscoverValidationContext(assembly);
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
            if(DVcontext == null) Initialize();
            var validatorStrategyHandler = CreateInstanceFactory.CreateValidatorStrategyHandler<T>();
            var validator = useThisValidatorType == null
                ? validatorStrategyHandler.GetValidator(DVcontext, element)
                : validatorStrategyHandler.GetValidator(DVcontext, useThisValidatorType);

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
            if (DVcontext == null) Initialize();
            return elements.Select(element => ValidateEntity(element, useThisValidatorType)).ToList();
        }

        public static List<IData<T>> ValidateEntityAsync<T>(List<T> elements, Type useThisValidatorType = null)
        {
            var validatorStrategyHandler = CreateInstanceFactory.CreateValidatorStrategyHandler<T>();
            var validator = useThisValidatorType == null
                ? validatorStrategyHandler.GetValidator(DVcontext, elements.First())
                : validatorStrategyHandler.GetValidator(DVcontext, useThisValidatorType);

            IList<Task<IData<T>>> tasks = new List<Task<IData<T>>>();
            elements.ForEach(element =>
            {
                tasks.Add(Task.Run(() => validatorStrategyHandler.ValidateOneTypeEntity(element, DVcontext, validator)));
            });

            return tasks.Select(t => t.Result).ToList();
        }
        
        public static DiscoverValidationResults ValidateMultipleEntities<T>(IList<T> entities)
        {
            if (DVcontext == null) Initialize();
            if(DVcontext?.DiscoverValidationResults == null) DVcontext = CreateInstanceFactory.InitializeDiscoverValidationResults(DVcontext);
            var validatorStrategyHandler = CreateInstanceFactory.CreateValidatorStrategyHandler<T>();

            DVcontext.ValidatorsTypesDictionary
                .Where(d => entities
                    .Select(e => e.GetType())
                    .Distinct().Contains(d.Key))
                .ForEach(d =>
                {
                    if (!DVcontext.ValidatorsInstancesDictionary.ContainsKey(d.Key))
                        DVcontext.RegisterValidatorInstance(d.Key, CreateInstanceFactory.CreateValidator(d.Value));
                });

            entities.ForEach(entity =>
            {
                ValidationResult validationResult = null;
                if (DVcontext.ValidatorsInstancesDictionary.ContainsKey(entity.GetType()))
                {
                    validationResult =
                        DVcontext.ValidatorsInstancesDictionary[entity.GetType()]
                            .ValidateEntity(entity);
                }
                validatorStrategyHandler
                    .UpdateValidationResulsImproved(DVcontext, entity, validationResult);
            });
            
            return DVcontext.DiscoverValidationResults;
        }

        public static DiscoverValidationResults ValidateMultipleEntitiesAsync<T>(IList<T> entities)
        {
            if (DVcontext == null) Initialize();
            DVcontext = CreateInstanceFactory.InitializeDiscoverValidationResults(DVcontext);
            var validatorStrategyHandler = CreateInstanceFactory.CreateValidatorStrategyHandler<T>();
            var tasks = new List<Task>();
            
            DVcontext.ValidatorsTypesDictionary
                .Where(d => entities
                    .Select(e => e.GetType())
                    .Distinct().Contains(d.Key))
                .ForEach(d => tasks.Add(Task.Run(() =>
                {
                    if (!DVcontext.ValidatorsInstancesDictionary.ContainsKey(d.Key))
                        DVcontext.RegisterValidatorInstance(d.Key, CreateInstanceFactory.CreateValidator(d.Value));
                })));

            tasks.ForEach(t=>t.Wait());
            tasks.Clear();

            entities.ForEach(entity =>
            {
                tasks.Add(Task.Run(() =>
                {
                    ValidationResult validationResult = null;
                    if (DVcontext.ValidatorsInstancesDictionary.ContainsKey(entity.GetType()))
                    {
                        validationResult =
                            DVcontext.ValidatorsInstancesDictionary[entity.GetType()]
                                .ValidateEntity(entity);
                    }

                    validatorStrategyHandler
                        .UpdateValidationResulsImprovedLock(DVcontext, entity, validationResult);
                }));
            });

            tasks.ForEach(t => t.Wait());
            return DVcontext.DiscoverValidationResults;
        }
        
        #region Obsolete

        /// <summary>
        /// Validate a list of entities of any type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities">Entities to validate</param>
        /// <returns>Returns a DiscoverValidationResults with all the validations results</returns>
        [Obsolete("I don't like it")]
        public static DiscoverValidationResults ValidateMultipleEntitiesOld<T>(IList<T> entities)
        {
            if (DVcontext == null) Initialize();
            DVcontext = CreateInstanceFactory.InitializeDiscoverValidationResults(DVcontext);
            var validatorStrategyHandler = CreateInstanceFactory.CreateValidatorStrategyHandler<T>();

            entities.ForEach(element => validatorStrategyHandler.UpdateValidationResuls(DVcontext, element));

            return DVcontext.DiscoverValidationResults;
        }

        [Obsolete("I don't like it")]
        public static DiscoverValidationResults ValidateMultipleEntitiesAssyncOld<T>(IList<T> entities)
        {
            if (DVcontext == null) Initialize();
            IList<Task> tasks = new List<Task>();
            DVcontext = CreateInstanceFactory.InitializeDiscoverValidationResults(DVcontext);
            var validatorStrategyHandler = CreateInstanceFactory.CreateValidatorStrategyHandler<T>();

            entities.ForEach(element =>
                tasks.Add(Task.Run(() => ValidateEntityAndUpdateResults(element, validatorStrategyHandler)))
                );

            tasks.ForEach(t => t.Wait());
            return DVcontext.DiscoverValidationResults;
        }

        /// <summary>
        /// Validate one unique entity
        /// </summary>
        /// <typeparam name="T">Type of the entity to validate</typeparam>
        /// <param name="element">Entity to validate</param>
        /// <param name="useThisValidatorType">Optional parameter to use an specific validator. It has to implement IDiscoverValidator</param>
        /// <returns>Returns an IData of type T</returns>
        [Obsolete("Obsolete")]
        public static IData<T> ValidateEntityOld<T>(T element, Type useThisValidatorType = null)
        {
            //if(DVcontext == null) Initialize();
            var validatorStrategyHandler = CreateInstanceFactory.CreateValidatorStrategyHandler<T>();
            return validatorStrategyHandler.ValidateOneTypeEntityOld(element, DVcontext, useThisValidatorType);
        }

        [Obsolete("I don't like it")]
        private static void ValidateEntityAndUpdateResults<T>(T element, Strategy.ValidatorStrategyHanlder<T> validatorStrategyHandler)
        {
            var validator = validatorStrategyHandler.GetValidatorLock(DVcontext, element);
            ValidationResult validationResult = null;

            if (validator != null)
            {
                validationResult = validator.ValidateEntity(element);
            }

            validatorStrategyHandler.UpdateValidationResulsImprovedLock(DVcontext, element, validationResult);
        }
        #endregion
    }
}
