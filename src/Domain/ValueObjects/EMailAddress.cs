namespace Intec.Workshop1.Customers.Domain.ValueObjects;


public sealed record EMailAddress
{
    public string Value { get; private set; }

    // Ctor para EF (parámetroless)
    private EMailAddress() { }

    public EMailAddress(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email is required", nameof(value));

        if (!value.Contains("@"))
            throw new ArgumentException("Invalid email", nameof(value));

        Value = value;
    }
}
