using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ValidationAttributeCore.CustomAttribute;
using ValidationAttributeCore.GenericValidator;
using ValidationAttributeCore.Model.Interface;

namespace ValidationAttributeCore.Application
{
    public static class MyValidator
    {
        internal static Dictionary<Type, Type> ValidatorsDictionary;


        public static IList<IData<object>> Validate(IList<object> coleccion)
        {
            ValidatorsDictionary = new Dictionary<Type, Type>();

            LoadValidators();

            foreach (var element in coleccion)
            {
                if (ValidatorsDictionary.ContainsKey(element.GetType()))
                {
                    var validatorType =ValidatorsDictionary[element.GetType()];

                    //var constructedValidatorType = validatorType.MakeGenericType(element.GetType());

                    var validator = (IAttributeValidator) Activator.CreateInstance(validatorType);
                    var results = validator.ValidateEntity(element);
                }
            }

            return new List<IData<object>>();
        }

        public static IList<IData<T>> Validate<T>(T element)
        {
            ValidatorsDictionary = new Dictionary<Type, Type>();

            LoadValidators();

           // foreach (var element in coleccion)
            {
                if (ValidatorsDictionary.ContainsKey(element.GetType()))
                {
                    var validatorType = ValidatorsDictionary[element.GetType()];

                    //var constructedValidatorType = validatorType.MakeGenericType(element.GetType());

                    var validator = (IAttributeValidator)Activator.CreateInstance(validatorType);
                    var results = validator.ValidateEntity(element);
                }
            }

            return new List<IData<T>>();
        }

        private static void LoadValidators()
        {

            var aaa = AppDomain.CurrentDomain.GetAssemblies();
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
            ;


            //AppDomain.CurrentDomain.GetAssemblies()
            //    .SelectMany(s => s.GetTypes())
            //    .Where(t => typeof(IAttributeValidator).IsAssignableFrom(t))
            //    //.Where(t => typeof(AbstractAttributeValidator<>).IsAssignableFrom(t))
            //    //.Where(t => typeof(IData<>).IsAssignableFrom(t))
            //    .Where(t => !t.IsAbstract && !t.IsInterface)
            //    .Select(s => new
            //    {
            //        attribute = s.GetCustomAttributes<ValidateEntityAttribute>().Single(),
            //        validator = s
            //    })
            //    .Where(e => e.attribute != null).ToList()
            //    .ForEach(e => ValidatorsDictionary.Add(e.attribute.Entity, e.validator));
        }
        public static bool IsAssignableToGenericType(Type givenType, Type genericType)
        {
            var interfaceTypes = givenType.GetInterfaces();

            foreach (var it in interfaceTypes)
            {
                if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                    return true;
            }

            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                return true;

            Type baseType = givenType.BaseType;
            if (baseType == null) return false;

            return IsAssignableToGenericType(baseType, genericType);
        }


    }
}
