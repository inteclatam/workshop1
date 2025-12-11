namespace Intec.Workshop1.Customers.Infrastructure;

/// <summary>
/// Interface for generating unique identifiers.
/// </summary>
public interface IIdGenerator
{
    /// <summary>
    /// Generates a new unique identifier.
    /// </summary>
    /// <returns>A unique long identifier.</returns>
    long GenerateId();
}
