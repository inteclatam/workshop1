using Intec.Workshop1.Customers.Primitives;

namespace Intec.Workshop1.Customers.Domain.Events;

public class CustomerCreatedEvent : DomainEvent
{
    public CustomerCreatedEvent(long customerId, string firstName, string lastName, string email, string phoneNumber)
    {
        CustomerId = customerId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
    }

    public long CustomerId { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string Email { get; }
    public string PhoneNumber { get; }
}
