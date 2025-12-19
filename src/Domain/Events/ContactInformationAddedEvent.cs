using Intec.Workshop1.Customers.Primitives;

namespace Intec.Workshop1.Customers.Domain.Events;

public class ContactInformationAddedEvent : DomainEvent
{
    public ContactInformationAddedEvent(long customerId, long contactId, string email, string phoneNumber, bool isPrimary)
    {
        CustomerId = customerId;
        ContactId = contactId;
        Email = email;
        PhoneNumber = phoneNumber;
        IsPrimary = isPrimary;
    }

    public long CustomerId { get; }
    public long ContactId { get; }
    public string Email { get; }
    public string PhoneNumber { get; }
    public bool IsPrimary { get; }
}
