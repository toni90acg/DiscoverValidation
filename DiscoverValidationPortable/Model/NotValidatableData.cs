using System.Collections.Generic;
using DiscoverValidationPortable.Model.Interface;
using FluentValidation.Results;

namespace DiscoverValidationPortable.Model
{
    public class NotValidatableData<T> : IData<T>
    {
        public T Entity { get; set; }

        public NotValidatableData(T entity)
        {
            Entity = entity;
        }

        public bool? IsValid()
        {
            return null;
        }

        public IList<ValidationFailure> GetValidationFailures()
        {
            return null;
        }
    }
}
