using System.Collections.Generic;
using DiscoverValidation.Model.Data.Interface;
using FluentValidation.Results;

namespace DiscoverValidation.Model.Data
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
            return new List<ValidationFailure>()
            {
                new ValidationFailure(nameof(Entity),"No validators have been found")
            };
        }
    }
}
