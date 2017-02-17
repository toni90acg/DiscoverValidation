using ValidationAttributeCore.Model.Interface;

namespace ValidationAttributeCore.Model
{
    public class ValidData<T> : IData<T>
    {
        public T Entity { get; set; }

        public ValidData(T entity)
        {
            Entity = entity;
        }
    }
}