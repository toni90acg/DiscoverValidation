using System.Collections.Generic;
using FluentValidation.Results;
using ValidationAttributeCore.Model.Interface;

namespace ValidationAttributeCore.Model
{
    public class InvalidData<T> : IData<T>
    {
        public T Entity { get; set; }
        public IList<ValidationFailure> ValidationFailures { get; set; }

        public InvalidData(T entity)
        {
            Entity = entity;
        }
    }
}