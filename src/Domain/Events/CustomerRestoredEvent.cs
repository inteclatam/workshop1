using Intec.Workshop1.Customers.Primitives;

namespace Intec.Workshop1.Customers.Domain.Events;

public class CustomerRestoredEvent : DomainEvent
{
    public CustomerRestoredEvent(long customerId)
    {
        CustomerId = customerId;
    }

    public long CustomerId { get; }
}
