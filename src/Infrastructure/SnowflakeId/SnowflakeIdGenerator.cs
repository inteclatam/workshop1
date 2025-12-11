namespace Intec.Workshop1.Customers.Infrastructure.SnowflakeId;

/// <summary>
/// Implementation of IIdGenerator using Snowflake algorithm.
/// Generates unique distributed IDs based on timestamp, worker ID, and sequence.
/// </summary>
public class SnowflakeIdGenerator : IIdGenerator
{
    private readonly IIdGeneratorPool _idGeneratorPool;

    public SnowflakeIdGenerator(IIdGeneratorPool idGeneratorPool)
    {
        _idGeneratorPool = idGeneratorPool ?? throw new ArgumentNullException(nameof(idGeneratorPool));
    }

    /// <summary>
    /// Generates a new unique identifier using Snowflake algorithm.
    /// </summary>
    /// <returns>A unique long identifier.</returns>
    public long GenerateId()
    {
        return _idGeneratorPool.NextId();
    }
}
