using System.Collections.Generic;
using DiscoverValidation.Model.Data.Interface;
using FluentValidation.Results;

namespace DiscoverValidation.Model.Data
{
    public class InvalidData<T> : IData<T>
    {
        public T Entity { get; set; }

        public IList<ValidationFailure> ValidationFailures { get; set; }

        public InvalidData(T entity)
        {
            Entity = entity;
        }

        public InvalidData(T entity, IList<ValidationFailure> validationFailures)
        {
            Entity = entity;
            ValidationFailures = validationFailures;
        }

        public bool? IsValid()
        {
            return false;
        }

        public IList<ValidationFailure> GetValidationFailures()
        {
            return ValidationFailures;
        }
    }
}