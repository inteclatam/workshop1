using Intec.Workshop1.Customers.Domain.ValueObjects;

namespace Intec.Workshop1.Customers.Domain;

public class Customer: Aggregate<CustomerId>,IHaveAudit, IHaveCreator, IHaveSoftDelete
{
    public Customer()
    {
        
    }

    public CustomerId CustomerId { get; set; }
    public CustomerName Name{get;set;}
public ContactInformation ContactInformation{get;set;}
//Audit Info
    public DateTime Created { get; }
    public int? CreatedBy { get; }
    public DateTime? LastModified { get; }
    public int? LastModifiedBy { get; }

    public void Create(CustomerName name,string email,
        string phoneNumber)
    {
        Name = name;
        ContactInformation.UpdateEmailAddress(email);
        ContactInformation.UpdatePhonenumber(phoneNumber);
    }
   
}