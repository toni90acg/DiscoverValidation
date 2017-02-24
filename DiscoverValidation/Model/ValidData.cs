using System.Collections.Generic;
using DiscoverValidation.Model.Interface;
using FluentValidation.Results;

namespace DiscoverValidation.Model
{
    public class ValidData<T> : IData<T>
    {
        public T Entity { get; set; }

        public ValidData(T entity)
        {
            Entity = entity;
        }

        public bool? IsValid()
        {
            return true;
        }

        public IList<ValidationFailure> GetValidationFailures()
        {
            return null;
        }
    }
}