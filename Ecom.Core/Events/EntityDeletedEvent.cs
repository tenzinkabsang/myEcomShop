namespace Ecom.Core.Events;

public class EntityDeletedEvent<T>(T entity) where T: BaseEntity
{
    public T Entity { get; } = entity;
}
