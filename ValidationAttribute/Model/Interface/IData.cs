using System;

namespace ValidationAttribute.Model.Interface
{
    public interface IData
    {
        Type Type { get; set; }
        object Entity { get; set; }
    }
}