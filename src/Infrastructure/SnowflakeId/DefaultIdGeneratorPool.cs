using System;

namespace Intec.Workshop1.Customers.Infrastructure.SnowflakeId;

/// <summary>
/// Default implementation of IIdGeneratorPool using Snowflake algorithm.
/// Generates unique distributed IDs based on timestamp, worker ID, datacenter ID, and sequence.
/// </summary>
public sealed class DefaultIdGeneratorPool : IIdGeneratorPool
{
    // Epoch propia (puedes cambiarla si quieres)
    private static readonly DateTimeOffset Epoch =
        new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero);

    private const int WorkerIdBits = 5;
    private const int DatacenterIdBits = 5;
    private const int SequenceBits = 12;

    private const long MaxWorkerId = -1L ^ (-1L << WorkerIdBits);         // 31
    private const long MaxDatacenterId = -1L ^ (-1L << DatacenterIdBits); // 31
    private const long SequenceMask = -1L ^ (-1L << SequenceBits);        // 4095

    private const int WorkerIdShift = SequenceBits;                                // 12
    private const int DatacenterIdShift = SequenceBits + WorkerIdBits;            // 17
    private const int TimestampLeftShift = SequenceBits + WorkerIdBits + DatacenterIdBits; // 22

    private readonly object _lock = new();

    private readonly long _workerId;
    private readonly long _datacenterId;

    private long _lastTimestamp = -1L;
    private long _sequence = 0L;

    /// <summary>
    /// Initializes a new instance of the DefaultIdGeneratorPool class.
    /// </summary>
    /// <param name="options">Configuration options for the ID generator.</param>
    /// <exception cref="ArgumentNullException">Thrown when options is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when workerId or datacenterId are out of valid range.</exception>
    public DefaultIdGeneratorPool(IdGeneratorOptions options)
    {
        if (options == null)
            throw new ArgumentNullException(nameof(options));

        if (options.WorkerId < 0 || options.WorkerId > MaxWorkerId)
            throw new ArgumentOutOfRangeException(nameof(options.WorkerId),
                $"workerId debe estar entre 0 y {MaxWorkerId}");

        if (options.DatacenterId < 0 || options.DatacenterId > MaxDatacenterId)
            throw new ArgumentOutOfRangeException(nameof(options.DatacenterId),
                $"datacenterId debe estar entre 0 y {MaxDatacenterId}");

        _workerId = options.WorkerId;
        _datacenterId = options.DatacenterId;
    }

    /// <summary>
    /// Generates the next unique identifier using Snowflake algorithm.
    /// </summary>
    /// <returns>A unique long identifier.</returns>
    /// <exception cref="InvalidOperationException">Thrown when clock moves backwards.</exception>
    public long NextId()
    {
        lock (_lock)
        {
            var timestamp = GetCurrentTimestamp();

            if (timestamp < _lastTimestamp)
                throw new InvalidOperationException("El reloj se movió hacia atrás.");

            if (timestamp == _lastTimestamp)
            {
                _sequence = (_sequence + 1) & SequenceMask;

                if (_sequence == 0)
                    timestamp = WaitNextMillis(timestamp);
            }
            else
            {
                _sequence = 0L;
            }

            _lastTimestamp = timestamp;

            var id =
                (timestamp << TimestampLeftShift) |
                (_datacenterId << DatacenterIdShift) |
                (_workerId << WorkerIdShift) |
                _sequence;

            return id;
        }
    }

    private static long GetCurrentTimestamp()
        => (long)(DateTimeOffset.UtcNow - Epoch).TotalMilliseconds;

    private static long WaitNextMillis(long currentTimestamp)
    {
        var ts = GetCurrentTimestamp();
        while (ts <= currentTimestamp)
            ts = GetCurrentTimestamp();
        return ts;
    }
}
