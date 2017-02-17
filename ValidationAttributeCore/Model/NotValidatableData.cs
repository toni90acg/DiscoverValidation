using ValidationAttributeCore.Model.Interface;

namespace ValidationAttributeCore.Model
{
    public class NotValidatableData<T> : IData<T>
    {
        public T Entity { get; set; }

        public NotValidatableData(T entity)
        {
            Entity = entity;
        }
    }
}
