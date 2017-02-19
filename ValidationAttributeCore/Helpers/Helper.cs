using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ValidationAttributeCore.CustomAttribute;
using ValidationAttributeCore.GenericValidator;

namespace ValidationAttributeCore.Helpers
{
    internal static class Helper
    {
        internal static bool IsAssignableToGenericType(Type givenType, Type genericType)
        {
            var interfaceTypes = givenType.GetInterfaces();

            foreach (var it in interfaceTypes)
            {
                if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                    return true;
            }

            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                return true;

            var baseType = givenType.BaseType;
            if (baseType == null) return false;

            return IsAssignableToGenericType(baseType, genericType);
        }

        internal static Dictionary<Type, Type> LoadValidators()
        {
            var validatorsDictionary = new Dictionary<Type, Type>();

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
                .ForEach(e => validatorsDictionary.Add(e.attribute.Entity, e.validator));

            return validatorsDictionary;
        }
    }
}
