using System;
using ValidationAttributeCore.Model.Interface;

namespace ValidationAttributeCore.Model
{
    public class NotValidatableData<T> : IData<T>
    {
        public Type Type { get; set; }
        public T Entity { get; set; }
        public T Entity2 { get; set; }
    }
}
