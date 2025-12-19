using Intec.Workshop1.Customers.Primitives;

namespace Intec.Workshop1.Customers.Domain.Events;

public class ContactRemovedEvent : DomainEvent
{
    public ContactRemovedEvent(long customerId, long contactId, string email, string phoneNumber)
    {
        CustomerId = customerId;
        ContactId = contactId;
        Email = email;
        PhoneNumber = phoneNumber;
    }

    public long CustomerId { get; }
    public long ContactId { get; }
    public string Email { get; }
    public string PhoneNumber { get; }
}
