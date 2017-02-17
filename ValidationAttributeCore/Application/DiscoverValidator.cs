using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using ValidationAttributeCore.CustomAttribute;
using ValidationAttributeCore.GenericValidator;
using ValidationAttributeCore.Model;
using ValidationAttributeCore.Model.Interface;

namespace ValidationAttributeCore.Application
{
    public static class DiscoverValidator
    {
        internal static Dictionary<Type, Type> ValidatorsDictionary;

        static DiscoverValidator()
        {
            ValidatorsDictionary = new Dictionary<Type, Type>();

            LoadValidators();
        }

        public static IList<object> ValidateAll(IList<object> coleccion)
        {
            var result = new List<object>();

            foreach (var element in coleccion)
            {
                if (ValidatorsDictionary.ContainsKey(element.GetType()))
                {
                    var validatorType = ValidatorsDictionary[element.GetType()];

                    var validator = (IAttributeValidator) Activator.CreateInstance(validatorType);
                    var validationResult = validator.ValidateEntity(element);

                    if (validationResult == null)
                    {
                        result.Add(CreateDataObj(typeof(NotValidatableData<>), element));
                    }

                    else if (validationResult.IsValid)
                    {
                        result.Add(CreateDataObj(typeof(ValidData<>), element));
                    }
                    else
                    {
                        result.Add(CreateDataObj(typeof(InvalidData<>), element));
                    }
                }
                else
                {
                    result.Add(CreateDataObj(typeof(NotValidatableData<>), element));
                }
            }

            return result;
        }
        
        private static IData<T> Validate<T>(T element)
        {
            return ValidateElement(element);
        }

        public static IList<IData<T>> GetDataOfType<T>(IList<object> lista)
        {
            var result = new List<IData<T>>();
            foreach (var o in lista)
            {
                
                var type = o.GetType();
                if (type == typeof(ValidData<T>))
                {
                    var element = o as IData<T>;
                    T element2 =  element.Entity;
                    result.Add(CreateData<T>(typeof(ValidData<>), element2));
                }
            }
            return result;
        }

        public static IData<T> ValidateElement<T>(T element)
        {
            if (ValidatorsDictionary.ContainsKey(element.GetType()))
            {
                var validatorType = ValidatorsDictionary[element.GetType()];

                var validator = (IAttributeValidator) Activator.CreateInstance(validatorType);
                var results = validator.ValidateEntity(element);

                if (results.IsValid)
                {
                    return CreateData(typeof(ValidData<>), element);
                }
                else
                {
                    return CreateData(typeof(InvalidData<>), element);
                }
            }
            return CreateData(typeof(NotValidatableData<>), element);
        }

        private static IData<T> CreateData<T>(Type typeOfData, T element)
        {
            return (IData<T>) CreateDataObj(typeOfData, element);
        }


        private static object CreateDataObj(Type typeOfData, object element)
        {
            Type[] typeArgs = { element.GetType() };
            var makeme = typeOfData.MakeGenericType(typeArgs);
            return Activator.CreateInstance(makeme, element);
        }

        private static void LoadValidators()
        {
            AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                   .Where(t => IsAssignableToGenericType(t, typeof(AbstractAttributeValidator<>)))
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .Select(s => new
                {
                    attribute = s.GetCustomAttributes<ValidateEntityAttribute>().Single(),
                    validator = s
                })
                .Where(e => e.attribute != null).ToList()
                .ForEach(e => ValidatorsDictionary.Add(e.attribute.Entity, e.validator));
        }

        private static bool IsAssignableToGenericType(Type givenType, Type genericType)
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
    }
}
