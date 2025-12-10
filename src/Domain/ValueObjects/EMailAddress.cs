namespace Intec.Workshop1.Customers.Domain.ValueObjects;

public readonly record struct EMailAddress
{
    
    public string Value { get; }

    public EMailAddress(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email is required", nameof(value));

        if (!value.Contains("@"))
            throw new ArgumentException("Invalid email", nameof(value));

        Value = value;
    }

    
    
   
}