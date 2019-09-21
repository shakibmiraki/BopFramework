namespace Bop.Core.Events
{
    public class EntityDeletedEvent<T> where T : BaseEntity
    {

        public EntityDeletedEvent(T entity)
        {
            this.Entity = entity;
        }

        public T Entity { get; }
    }
}
