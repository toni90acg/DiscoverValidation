using System;
using ValidationAttribute.Model.Data;

namespace ValidationAttribute.Model
{
    public class ValidData : IData
    {
        public Type Type { get; set; }
        public object Entity { get; set; }
    }
}