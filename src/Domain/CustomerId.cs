using Ardalis.GuardClauses;
using Intec.Workshop1.Customers.Primitives;

namespace Intec.Workshop1.Customers.Domain;

public record CustomerId:AggregateId
{
    public CustomerId(long value) : base(value)
    {
        Guard.Against.NegativeOrZero(value, nameof(value));
    }
}