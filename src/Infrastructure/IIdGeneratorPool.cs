namespace Intec.Workshop1.Customers.Infrastructure;

/// <summary>
/// Interface for ID generator pool that manages ID generation with configurable options.
/// </summary>
public interface IIdGeneratorPool
{
    /// <summary>
    /// Generates the next unique identifier.
    /// </summary>
    /// <returns>A unique long identifier.</returns>
    long NextId();
}
