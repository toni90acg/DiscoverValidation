using System.Collections.Generic;
using DiscoverValidationCore.Model.Interface;
using FluentValidation.Results;

namespace DiscoverValidationCore.Model
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
