namespace Bop.Core.Events
{
    public class EntityUpdatedEvent<T> where T : BaseEntity
    {
        public EntityUpdatedEvent(T entity)
        {
            Entity = entity;
        }

        public T Entity { get; }
    }
}
