using System;
using System.Collections.Generic;
using FluentValidation.Results;
using ValidationAttributeCore.Model.Interface;

namespace ValidationAttributeCore.Helpers
{
    internal static class CreateInstanceHelper
    {
        internal static IData<T> CreateDataCasted<T>(Type typeOfData, T element, IList<ValidationFailure> failures = null)
        {
            return (IData<T>)CreateData(typeOfData, element, failures);
        }

        internal static object CreateData(Type typeOfData, object element, IList<ValidationFailure> failures = null)
        {
            Type[] typeArgs = { element.GetType() };
            var makeme = typeOfData.MakeGenericType(typeArgs);

            if (failures == null)
            {
                return Activator.CreateInstance(makeme, element);
            }
            var ctorParams = new[]
            {
                element,
                failures
            };
            return Activator.CreateInstance(makeme, ctorParams);
        }
    }
}
