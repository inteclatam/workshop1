using Intec.Workshop1.Customers.Primitives;

namespace Intec.Workshop1.Customers.Domain.Events;

public class PrimaryContactChangedEvent : DomainEvent
{
    public PrimaryContactChangedEvent(long customerId, long? previousPrimaryContactId, long newPrimaryContactId)
    {
        CustomerId = customerId;
        PreviousPrimaryContactId = previousPrimaryContactId;
        NewPrimaryContactId = newPrimaryContactId;
    }

    public long CustomerId { get; }
    public long? PreviousPrimaryContactId { get; }
    public long NewPrimaryContactId { get; }
}
