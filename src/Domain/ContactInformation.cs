using Ardalis.GuardClauses;
using Intec.Workshop1.Customers.Domain.ValueObjects;
using Intec.Workshop1.Customers.Primitives;

namespace Intec.Workshop1.Customers.Domain;

public class ContactInformation : Entity<long>
{
    // Ctor para EF
    public ContactInformation()
    {
    }

    public EMailAddress Email { get; private set; }
    public PhoneNumber? PhoneNumber { get; private set; }
    public bool IsVerified { get; private set; }
    public bool IsPrimary { get; private set; }

    // Foreign Key
    public long CustomerId { get; private set; }

    public void UpdateEmailAddress(string email)
    {
        Guard.Against.NullOrWhiteSpace(email, nameof(email));

        Email = new EMailAddress(email);
        IsVerified = false;
    }

    public void UpdatePhonenumber(string phoneNumber)
    {
        Guard.Against.NullOrWhiteSpace(phoneNumber, nameof(phoneNumber));

        // "57+3001234567"
        var phone = phoneNumber.Split('+', StringSplitOptions.RemoveEmptyEntries);
        if (phone.Length != 2)
            throw new ArgumentException("Invalid prefix+number format", nameof(phoneNumber));

        var prefix = phone[0];
        var number = phone[1];

        PhoneNumber = new PhoneNumber(number, prefix);
        IsVerified = false;
    }

    public void Verify()
    {
        IsVerified = true;
    }

    public void SetAsPrimary()
    {
        IsPrimary = true;
    }

    public void UnsetPrimary()
    {
        IsPrimary = false;
    }

    public void SetCustomerId(long customerId)
    {
        CustomerId = customerId;
    }

    public void SetId(long id)
    {
        Id = id;
    }
}