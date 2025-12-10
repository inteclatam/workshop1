public class Customer:Entity, IHaveSoftDelete
{
    public CustomerId CustomerId { get; set; }


    public CustomerName Name{get;set;}
    public EMailAddress Email { get; set; }

    public PhoneNumber PhoneNumber { get; set; }

    public DateTime CreatedAt { get; set; }
}