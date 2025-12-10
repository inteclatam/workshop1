

public interface IIdentity<out TId>
{
    /// <summary>
    /// Gets the generic identifier.
    /// </summary>
    public TId Value { get; }
}