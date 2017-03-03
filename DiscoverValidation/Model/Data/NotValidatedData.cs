using System;
using System.Collections.Generic;
using DiscoverValidation.Model.Data.Interface;
using FluentValidation.Results;

namespace DiscoverValidation.Model.Data
{
    public class NotValidatedData<T> : IData<T>
    {
        public T Entity { get; set; }

        public List<Type> ValidatorsFound { get; set; }

        public NotValidatedData(T entity)
        {
            Entity = entity;
        }

        public NotValidatedData(T entity, List<Type> validatorsFound)
        {
            Entity = entity;
            ValidatorsFound = validatorsFound;
        }

        public bool? IsValid()
        {
            return false;
        }

        public IList<ValidationFailure> GetValidationFailures()
        {
            var validationFailures = new List<ValidationFailure>()
            {
                new ValidationFailure(nameof(Entity),"More than one validator have been found")
            };

            ValidatorsFound.ForEach(v=> validationFailures.Add(new ValidationFailure(nameof(Entity),$"Validator: {nameof(v)}")));

            return validationFailures;
        }

    }
}
