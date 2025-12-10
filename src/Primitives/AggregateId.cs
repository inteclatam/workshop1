using Ardalis.GuardClauses;

namespace MVCT.Terra.CommonV1.Domain.Primitives;

public record AggregateId<T> : Identity<T>
{
    public AggregateId(T value) : base(value)
    {
    }

    public static implicit operator T(AggregateId<T> id) => Guard.Against.Null(id.Value, nameof(id.Value));
    public static implicit operator AggregateId<T>(T id) => new(id);
}

public record AggregateId : AggregateId<int>
{
    public AggregateId(int value) : base(value)
    {
        Guard.Against.NegativeOrZero(value, nameof(value));
    }

    public static implicit operator int(AggregateId id) => Guard.Against.Null(id.Value, nameof(id.Value));
    public static implicit operator AggregateId(int id) => new(id);
}