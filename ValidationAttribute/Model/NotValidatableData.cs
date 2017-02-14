using System;
using ValidationAttribute.Model.Interface;

namespace ValidationAttribute.Model
{
    public class NotValidatableData : IData
    {
        public Type Type { get; set; }
        public object Entity { get; set; }
    }
}
