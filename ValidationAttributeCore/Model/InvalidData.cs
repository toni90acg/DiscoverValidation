using System;
using System.Collections.Generic;
using FluentValidation.Results;
using ValidationAttributeCore.Model.Interface;

namespace ValidationAttributeCore.Model
{
    public class InvalidData<T> : IData<T>
    {
        public Type Type { get; set; }
        public T Entity { get; set; }
        public IList<ValidationFailure> ValidationFailures { get; set; }
    }
}