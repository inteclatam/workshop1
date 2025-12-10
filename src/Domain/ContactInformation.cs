using Ardalis.GuardClauses;
using Intec.Workshop1.Customers.Domain.ValueObjects;

namespace Intec.Workshop1.Customers.Domain;

public  class ContactInformation:Entity<long>
{
    public EMailAddress Email { get; set; }

    public PhoneNumber? PhoneNumber { get; set; } = null!;
    public bool IsVerified { get; set; }
    public ContactInformation()
    {
        
    }

    public ContactInformation(EMailAddress email, PhoneNumber phoneNumber)
    {
        Guard.Against.Null(email, nameof(email));
        Guard.Against.Null(phoneNumber, nameof(phoneNumber));
       Email = email;
       PhoneNumber = phoneNumber;
    }
    
    public void Verify()
    {
        IsVerified = true;
    }
    public void UpdateEmailAddress(string newEmail)
    {
        Email= new EMailAddress( newEmail);
        IsVerified = false;
    }

    public void UpdatePhonenumber(string phoneNumber)
    {
        var phone = phoneNumber.Split('+');
        var number = phone[1];
        var prefix = phone[0];
        
        PhoneNumber=new PhoneNumber(prefix,number);
        IsVerified = false;
    }

    
    
}