using Intec.Workshop1.Customers.Primitives;

namespace Intec.Workshop1.Customers.Domain.Events;

public class ContactEmailUpdatedEvent : DomainEvent
{
    public ContactEmailUpdatedEvent(long customerId, long contactId, string oldEmail, string newEmail)
    {
        CustomerId = customerId;
        ContactId = contactId;
        OldEmail = oldEmail;
        NewEmail = newEmail;
    }

    public long CustomerId { get; }
    public long ContactId { get; }
    public string OldEmail { get; }
    public string NewEmail { get; }
}
