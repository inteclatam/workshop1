namespace Intec.Workshop1.Customers.Domain.ValueObjects;

public sealed record  PhoneNumber
{
    public string Value { get; }
    public string Number { get; }
    public string Prefix { get; }

    public PhoneNumber(string number, string prefix)
    {
        if (string.IsNullOrWhiteSpace(number))
            throw new ArgumentException("Phone number is required", nameof(number));

        if (string.IsNullOrWhiteSpace(prefix))
            throw new ArgumentException("Phone prefix is required", nameof(number));
        Number = number;
        Prefix = prefix;

        prefix = prefix.Length > 10 ? prefix.Substring(0, 10) : prefix;
        number = number.Length > 10 ? number.Substring(0, 10) : number;
        Value = string.Concat(prefix, number);
    }
}
