using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DiscoverValidationCore.CustomAttribute;
using DiscoverValidationCore.GenericValidator;

namespace DiscoverValidationCore.Helpers
{
    internal static class AssembliesHelper
    {
        internal static Dictionary<Type, Type> LoadValidators()
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

        internal static Dictionary<Type, Type> LoadValidatorsOld()
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
