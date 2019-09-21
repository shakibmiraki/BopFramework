namespace Bop.Core.Events
{
    public class EntityInsertedEvent<T> where T : BaseEntity
    {
        public EntityInsertedEvent(T entity)
        {
            Entity = entity;
        }

        public T Entity { get; }
    }
}
