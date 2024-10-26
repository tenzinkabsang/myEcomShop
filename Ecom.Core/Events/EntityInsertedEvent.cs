namespace Ecom.Core.Events;

public class EntityInsertedEvent<T>(T entity) where T : BaseEntity
{
    public T Entity { get; } = entity;
}
