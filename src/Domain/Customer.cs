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

    // Información de contacto - Relación uno-a-muchos
    private readonly List<ContactInformation> _contactInformations = new();
    public IReadOnlyCollection<ContactInformation> ContactInformations => _contactInformations.AsReadOnly();

    // Propiedades de conveniencia para el contacto principal
    public ContactInformation? PrimaryContact => _contactInformations.FirstOrDefault(c => c.IsPrimary);
    public EMailAddress? Email => PrimaryContact?.Email;
    public PhoneNumber? PhoneNumber => PrimaryContact?.PhoneNumber;
    public bool IsContactVerified => PrimaryContact?.IsVerified ?? false;

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

        // Agregar contacto principal
        customer.AddContactInformation(email, phoneNumber, isPrimary: true);

        return customer;
    }

    // Comportamiento de dominio

    public void UpdateName(string firstName, string lastName, int? userId = null)
    {
        Name = new CustomerName(firstName, lastName);
        Touch(userId);
    }

    public ContactInformation AddContactInformation(string email, string phoneNumber, bool isPrimary = false, int? userId = null)
    {
        // Si se marca como primario, desmarcar el anterior
        if (isPrimary)
        {
            foreach (var contact in _contactInformations)
            {
                contact.UnsetPrimary();
            }
        }

        var contactInfo = new ContactInformation();
        contactInfo.UpdateEmailAddress(email);
        contactInfo.UpdatePhonenumber(phoneNumber);

        if (isPrimary)
        {
            contactInfo.SetAsPrimary();
        }

        _contactInformations.Add(contactInfo);
        Touch(userId);

        return contactInfo;
    }

    public void UpdateContactEmail(long contactId, string email, int? userId = null)
    {
        var contact = _contactInformations.FirstOrDefault(c => c.Id == contactId);
        if (contact == null)
            throw new InvalidOperationException($"Contact information with id {contactId} not found");

        contact.UpdateEmailAddress(email);
        Touch(userId);
    }

    public void UpdateContactPhoneNumber(long contactId, string phoneNumber, int? userId = null)
    {
        var contact = _contactInformations.FirstOrDefault(c => c.Id == contactId);
        if (contact == null)
            throw new InvalidOperationException($"Contact information with id {contactId} not found");

        contact.UpdatePhonenumber(phoneNumber);
        Touch(userId);
    }

    public void SetPrimaryContact(long contactId, int? userId = null)
    {
        var contact = _contactInformations.FirstOrDefault(c => c.Id == contactId);
        if (contact == null)
            throw new InvalidOperationException($"Contact information with id {contactId} not found");

        // Desmarcar todos los contactos como primarios
        foreach (var c in _contactInformations)
        {
            c.UnsetPrimary();
        }

        // Marcar el nuevo contacto como primario
        contact.SetAsPrimary();
        Touch(userId);
    }

    public void VerifyContact(long contactId, int? userId = null)
    {
        var contact = _contactInformations.FirstOrDefault(c => c.Id == contactId);
        if (contact == null)
            throw new InvalidOperationException($"Contact information with id {contactId} not found");

        contact.Verify();
        Touch(userId);
    }

    public void RemoveContactInformation(long contactId, int? userId = null)
    {
        var contact = _contactInformations.FirstOrDefault(c => c.Id == contactId);
        if (contact == null)
            throw new InvalidOperationException($"Contact information with id {contactId} not found");

        if (contact.IsPrimary && _contactInformations.Count > 1)
        {
            throw new InvalidOperationException("Cannot remove primary contact. Set another contact as primary first.");
        }

        _contactInformations.Remove(contact);
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
