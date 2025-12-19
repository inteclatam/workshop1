using Intec.Workshop1.Customers.Primitives;

namespace Intec.Workshop1.Customers.Domain.Events;

public class ContactVerifiedEvent : DomainEvent
{
    public ContactVerifiedEvent(long customerId, long contactId)
    {
        CustomerId = customerId;
        ContactId = contactId;
    }

    public long CustomerId { get; }
    public long ContactId { get; }
}
