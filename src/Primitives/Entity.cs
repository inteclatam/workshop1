
public abstract class Entity<TId> : IEntity<TId>
{
    public TId Id { get; protected init; } = default!;

}