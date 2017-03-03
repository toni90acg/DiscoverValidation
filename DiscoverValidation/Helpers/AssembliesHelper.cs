using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DiscoverValidation.CustomAttribute;
using DiscoverValidation.Extensions;
using DiscoverValidation.GenericValidator;
using DiscoverValidation.Model.ValidationResults;

namespace DiscoverValidation.Helpers
{
    public static class AssembliesHelper
    {
        internal static Dictionary<Type, Type> LoadValidators(out List<EntityWithMultipleValidators> entitiesWithMultipleValidatorsConflicts)
        {
            var validatorsDictionary = new Dictionary<Type, Type>();
            var entitiesWithMultipleValidators = new List<EntityWithMultipleValidators>();

            AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(t => IsAssignableToGenericType(t, typeof(AbstractDiscoverValidator<>)))
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .Select(s => new
                {
                    entity = s.BaseType?.GenericTypeArguments.Single(),
                    validator = s
                })
                .Where(e => e.entity != null).ToList()
                .ForEach(e =>
                {
                    if (validatorsDictionary.ContainsKey(e.entity))
                    {
                        var registeredEwmv =
                            entitiesWithMultipleValidators
                                .SingleOrDefault(ewmv => ewmv.EntityType == e.entity);
                        
                        if (registeredEwmv == null)
                            entitiesWithMultipleValidators.Add(
                                new EntityWithMultipleValidators(e.entity)
                                    .AddValidatorType(e.validator));
                        else
                            registeredEwmv.AddValidatorType(e.validator);
                    }
                    else
                    {
                        validatorsDictionary.Add(e.entity, e.validator);
                    }
                });

            entitiesWithMultipleValidators.ForEach(ewmv =>
            {
                var validatorType = validatorsDictionary[ewmv.EntityType];
                ewmv.AddValidatorType(validatorType);
                validatorsDictionary.Remove(ewmv.EntityType);
            });

            entitiesWithMultipleValidatorsConflicts = entitiesWithMultipleValidators;
            return validatorsDictionary;
        }

        internal static Dictionary<Type, Type> CreateValidators()
        {
            var validatorsDictionary = new Dictionary<Type, Type>();

            AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(t => IsAssignableToGenericType(t, typeof(AbstractDiscoverValidator<>)))
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .Select(s => new
                {
                    attribute = s.GetCustomAttributes<ValidateEntityAttribute>().Single(),
                    validator = s
                })
                .Where(e => e.attribute != null).ToList()
                .ForEach(e => validatorsDictionary.Add(e.attribute.Entity, e.validator));

            return validatorsDictionary;
        }

        private static bool IsAssignableToGenericType(Type givenType, Type genericType)
        {
            if (givenType.GetInterfaces()
                .Any(it => it.IsGenericType && it.GetGenericTypeDefinition() == genericType))
            {
                return true;
            }

            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                return true;

            var baseType = givenType.BaseType;

            return baseType != null && IsAssignableToGenericType(baseType, genericType);
        }
    }
}