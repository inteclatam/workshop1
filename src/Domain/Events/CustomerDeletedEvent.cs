using Intec.Workshop1.Customers.Primitives;

namespace Intec.Workshop1.Customers.Domain.Events;

public class CustomerDeletedEvent : DomainEvent
{
    public CustomerDeletedEvent(long customerId)
    {
        CustomerId = customerId;
    }

    public long CustomerId { get; }
}
