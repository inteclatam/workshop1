namespace Intec.Workshop1.Customers.Infrastructure.SnowflakeId;

/// <summary>
/// Configuration options for ID generator.
/// </summary>
public class IdGeneratorOptions
{
    /// <summary>
    /// Gets or sets the worker ID (machine ID) for distributed ID generation.
    /// Valid range: 0-31 (5 bits)
    /// </summary>
    public long WorkerId { get; set; }

    /// <summary>
    /// Gets or sets the datacenter ID for distributed ID generation.
    /// Valid range: 0-31 (5 bits)
    /// </summary>
    public long DatacenterId { get; set; }

    /// <summary>
    /// Gets or sets the custom epoch timestamp in milliseconds.
    /// Default: 1288834974657L (Twitter's epoch: Nov 04, 2010, 01:42:54 UTC)
    /// </summary>
    public long Epoch { get; set; } = 1288834974657L;

    /// <summary>
    /// Gets or sets the number of bits allocated for the sequence.
    /// Default: 12 bits (allowing 4096 IDs per millisecond)
    /// </summary>
    public int SequenceBits { get; set; } = 12;

    /// <summary>
    /// Gets or sets the number of bits allocated for worker ID.
    /// Default: 5 bits (allowing 32 workers)
    /// </summary>
    public int WorkerIdBits { get; set; } = 5;

    /// <summary>
    /// Gets or sets the number of bits allocated for datacenter ID.
    /// Default: 5 bits (allowing 32 datacenters)
    /// </summary>
    public int DatacenterIdBits { get; set; } = 5;
}
