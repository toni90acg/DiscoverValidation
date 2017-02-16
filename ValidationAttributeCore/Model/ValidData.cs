using System;
using ValidationAttributeCore.Model.Interface;

namespace ValidationAttributeCore.Model
{
    public class ValidData<T> : IData<T>
    {
        public Type Type { get; set; }
        public T Entity { get; set; }
    }
}