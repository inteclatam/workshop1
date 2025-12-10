using Ardalis.GuardClauses;

namespace Intec.Workshop1.Customers.Domain.ValueObjects;

public sealed record CustomerName
{
    private string FirstName { get; }
    private string LastName  { get; }
    public string FullName  => $"{FirstName} {LastName}";

    public CustomerName(string firstName, string lastName)
    {
        Guard.Against.NullOrEmpty(firstName, nameof(firstName));
        Guard.Against.NullOrEmpty(lastName, nameof(lastName));

        FirstName = firstName;
        LastName  = lastName;
    }
}