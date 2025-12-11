namespace Intec.Workshop1.Customers.Infrastructure.SnowflakeId;

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
