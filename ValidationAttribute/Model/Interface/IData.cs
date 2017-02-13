using System;

namespace ValidationAttribute.Model.Data
{
    public interface IData
    {
        Type Type { get; set; }
        Object Entity { get; set; }
    }
}