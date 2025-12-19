using Intec.Workshop1.Customers.Domain.Events;
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
        long id,
        long contactId,
        string firstName,
        string lastName,
        string email,
        string phoneNumber,
        int? userId = null)
    {
        var customer = new Customer
        {
            Id = new CustomerId(id),
            Name = new CustomerName(firstName, lastName),
            Created = DateTime.UtcNow,
            CreatedBy = userId
        };

        // Agregar contacto principal
        customer.AddContactInformation(contactId, email, phoneNumber, isPrimary: true);

        // Raise domain event
        customer.AddDomainEvent(new CustomerCreatedEvent(id, firstName, lastName, email, phoneNumber));

        return customer;
    }

    // Comportamiento de dominio

    public void UpdateName(string firstName, string lastName, int? userId = null)
    {
        var oldFirstName = Name.FirstName;
        var oldLastName = Name.LastName;

        Name = new CustomerName(firstName, lastName);
        Touch(userId);

        // Raise domain event
        AddDomainEvent(new CustomerNameChangedEvent(Id.Value, oldFirstName, oldLastName, firstName, lastName));
    }

    public ContactInformation AddContactInformation(long contactId, string email, string phoneNumber, bool isPrimary = false, int? userId = null)
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
        contactInfo.SetId(contactId);
        contactInfo.UpdateEmailAddress(email);
        contactInfo.UpdatePhonenumber(phoneNumber);

        if (isPrimary)
        {
            contactInfo.SetAsPrimary();
        }

        _contactInformations.Add(contactInfo);
        Touch(userId);

        // Raise domain event (only when not creating the customer)
        if (_contactInformations.Count > 1 || !isPrimary)
        {
            AddDomainEvent(new ContactInformationAddedEvent(Id.Value, contactId, email, phoneNumber, isPrimary));
        }

        return contactInfo;
    }

    public void UpdateContactEmail(long contactId, string email, int? userId = null)
    {
        var contact = _contactInformations.FirstOrDefault(c => c.Id == contactId);
        if (contact == null)
            throw new InvalidOperationException($"Contact information with id {contactId} not found");

        var oldEmail = contact.Email.Value;
        contact.UpdateEmailAddress(email);
        Touch(userId);

        // Raise domain event
        AddDomainEvent(new ContactEmailUpdatedEvent(Id.Value, contactId, oldEmail, email));
    }

    public void UpdateContactPhoneNumber(long contactId, string phoneNumber, int? userId = null)
    {
        var contact = _contactInformations.FirstOrDefault(c => c.Id == contactId);
        if (contact == null)
            throw new InvalidOperationException($"Contact information with id {contactId} not found");

        var oldPhoneNumber = contact.PhoneNumber.Value;
        contact.UpdatePhonenumber(phoneNumber);
        Touch(userId);

        // Raise domain event
        AddDomainEvent(new ContactPhoneUpdatedEvent(Id.Value, contactId, oldPhoneNumber, phoneNumber));
    }

    public void SetPrimaryContact(long contactId, int? userId = null)
    {
        var contact = _contactInformations.FirstOrDefault(c => c.Id == contactId);
        if (contact == null)
            throw new InvalidOperationException($"Contact information with id {contactId} not found");

        // Obtener el contacto primario anterior
        var previousPrimaryContact = _contactInformations.FirstOrDefault(c => c.IsPrimary);

        // Desmarcar todos los contactos como primarios
        foreach (var c in _contactInformations)
        {
            c.UnsetPrimary();
        }

        // Marcar el nuevo contacto como primario
        contact.SetAsPrimary();
        Touch(userId);

        // Raise domain event
        AddDomainEvent(new PrimaryContactChangedEvent(Id.Value, previousPrimaryContact?.Id, contactId));
    }

    public void VerifyContact(long contactId, int? userId = null)
    {
        var contact = _contactInformations.FirstOrDefault(c => c.Id == contactId);
        if (contact == null)
            throw new InvalidOperationException($"Contact information with id {contactId} not found");

        contact.Verify();
        Touch(userId);

        // Raise domain event
        AddDomainEvent(new ContactVerifiedEvent(Id.Value, contactId));
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

        var email = contact.Email.Value;
        var phoneNumber = contact.PhoneNumber.Value;

        _contactInformations.Remove(contact);
        Touch(userId);

        // Raise domain event
        AddDomainEvent(new ContactRemovedEvent(Id.Value, contactId, email, phoneNumber));
    }

    public void SoftDelete(int? userId = null)
    {
        if (IsDeleted) return;

        IsDeleted = true;
        Deleted = DateTime.UtcNow;
        DeletedBy = userId;

        // Raise domain event
        AddDomainEvent(new CustomerDeletedEvent(Id.Value));
    }

    public void Restore(int? userId = null)
    {
        if (!IsDeleted) return;

        IsDeleted = false;
        Deleted = null;
        DeletedBy = null;
        Touch(userId);

        // Raise domain event
        AddDomainEvent(new CustomerRestoredEvent(Id.Value));
    }

    private void Touch(int? userId)
    {
        LastModified = DateTime.UtcNow;
        LastModifiedBy = userId;
    }
}
