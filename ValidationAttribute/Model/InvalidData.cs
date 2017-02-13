using System;
using System.Collections.Generic;
using FluentValidation.Results;
using ValidationAttribute.Model.Data;

namespace ValidationAttribute.Model
{
    public class InvalidData : IData
    {
        public Type Type { get; set; }
        public object Entity { get; set; }
        public IList<ValidationFailure> ValidationFailures { get; set; }
    }
}