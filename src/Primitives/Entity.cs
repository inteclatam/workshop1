
namespace Intec.Workshop1.Customers.Primitives;

public abstract class Entity<TId> : IEntity<TId>
{
    public TId Id { get; protected init; } = default!;

}