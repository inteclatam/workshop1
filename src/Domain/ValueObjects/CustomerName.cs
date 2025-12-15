using Ardalis.GuardClauses;

namespace Intec.Workshop1.Customers.Domain.ValueObjects;

public sealed record CustomerName
{
    public string FirstName { get; private set; } = string.Empty;
    public string LastName  { get; private set; } = string.Empty;
    public string FullName  => $"{FirstName} {LastName}";

    public CustomerName()
    {

    }
    public CustomerName(string firstName, string lastName)
    {
        Guard.Against.NullOrEmpty(firstName, nameof(firstName));
        Guard.Against.NullOrEmpty(lastName, nameof(lastName));

        FirstName = firstName;
        LastName  = lastName;
    }
}