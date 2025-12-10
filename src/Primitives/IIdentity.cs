namespace MVCT.Terra.CommonV1.Domain.Primitives;

public interface IIdentity<out TId>
{
    /// <summary>
    /// Gets the generic identifier.
    /// </summary>
    public TId Value { get; }
}