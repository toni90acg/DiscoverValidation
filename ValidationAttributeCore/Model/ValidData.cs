using System.Collections.Generic;
using DiscoverValidationCore.Model.Interface;
using FluentValidation.Results;

namespace DiscoverValidationCore.Model
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