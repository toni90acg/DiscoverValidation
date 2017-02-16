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

        private static void LoadValidators()
        {


            AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(t => typeof(IFindValidation).IsAssignableFrom(t))
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
    }
}
