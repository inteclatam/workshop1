using Intec.Workshop1.Customers.Primitives;

namespace Intec.Workshop1.Customers.Domain.Events;

public class CustomerNameChangedEvent : DomainEvent
{
    public CustomerNameChangedEvent(long customerId, string oldFirstName, string oldLastName, string newFirstName, string newLastName)
    {
        CustomerId = customerId;
        OldFirstName = oldFirstName;
        OldLastName = oldLastName;
        NewFirstName = newFirstName;
        NewLastName = newLastName;
    }

    public long CustomerId { get; }
    public string OldFirstName { get; }
    public string OldLastName { get; }
    public string NewFirstName { get; }
    public string NewLastName { get; }
}
