using System;

namespace ValidationAttributeCore.Model.Interface
{
    public interface IData<T>
    {
        Type Type { get; set; }
        T Entity { get; set; }
    }
}