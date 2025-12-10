using Intec.Workshop1.Customers.Domain.ValueObjects;
using Intec.Workshop1.Customers.Primitives;

namespace Intec.Workshop1.Customers.Domain;

public class Customer : Aggregate<CustomerId>, IHaveAudit, IHaveSoftDelete
{
    // Ctor para EF
    public Customer()
    {
    }

    public CustomerName Name { get; private set; } = null!;

    // Información de contacto encapsulada
    private ContactInformation ContactInformation { get; } = new();

    // Facade sobre ContactInformation
    public EMailAddress Email => ContactInformation.Email;
    public PhoneNumber? PhoneNumber => ContactInformation.PhoneNumber;
    public bool IsContactVerified => ContactInformation.IsVerified;

    // Audit Info
    public DateTime Created { get; private set; }
    public int? CreatedBy { get; private set; }
    public DateTime? LastModified { get; private set; }
    public int? LastModifiedBy { get; private set; }

    // Soft delete
    public bool IsDeleted { get; set; }
    public DateTime? Deleted { get; set; }
    public int? DeletedBy { get; set; }

    // Factory de dominio
    public static Customer Create(
        string firstName,
        string lastName,
        string email,
        string phoneNumber,
        int? userId = null)
    {
        var customer = new Customer
        {
            Name = new CustomerName(firstName, lastName),
            Created = DateTime.UtcNow,
            CreatedBy = userId
        };

        customer.ContactInformation.UpdateEmailAddress(email);
        customer.ContactInformation.UpdatePhonenumber(phoneNumber);

        return customer;
    }

    // Comportamiento de dominio

    public void UpdateName(string firstName, string lastName, int? userId = null)
    {
        Name = new CustomerName(firstName, lastName);
        Touch(userId);
    }

    public void UpdateEmail(string email, int? userId = null)
    {
        ContactInformation.UpdateEmailAddress(email);
        Touch(userId);
    }

    public void UpdatePhoneNumber(string phoneNumber, int? userId = null)
    {
        ContactInformation.UpdatePhonenumber(phoneNumber);
        Touch(userId);
    }

    public void VerifyContact(int? userId = null)
    {
        ContactInformation.Verify();
        Touch(userId);
    }

    public void SoftDelete(int? userId = null)
    {
        if (IsDeleted) return;

        IsDeleted = true;
        Deleted = DateTime.UtcNow;
        DeletedBy = userId;
    }

    public void Restore(int? userId = null)
    {
        if (!IsDeleted) return;

        IsDeleted = false;
        Deleted = null;
        DeletedBy = null;
        Touch(userId);
    }

    private void Touch(int? userId)
    {
        LastModified = DateTime.UtcNow;
        LastModifiedBy = userId;
    }
}
