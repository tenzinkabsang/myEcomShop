namespace Ecom.Core.Events;

public class EntityUpdatedEvent<T>(T entity) where T : BaseEntity
{
    public T Entity { get; } = entity;
}
