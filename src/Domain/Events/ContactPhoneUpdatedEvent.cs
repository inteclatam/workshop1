using Intec.Workshop1.Customers.Primitives;

namespace Intec.Workshop1.Customers.Domain.Events;

public class ContactPhoneUpdatedEvent : DomainEvent
{
    public ContactPhoneUpdatedEvent(long customerId, long contactId, string oldPhoneNumber, string newPhoneNumber)
    {
        CustomerId = customerId;
        ContactId = contactId;
        OldPhoneNumber = oldPhoneNumber;
        NewPhoneNumber = newPhoneNumber;
    }

    public long CustomerId { get; }
    public long ContactId { get; }
    public string OldPhoneNumber { get; }
    public string NewPhoneNumber { get; }
}
