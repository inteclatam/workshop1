namespace MVCT.Terra.CommonV1.Domain.Primitives;

public interface IHaveCreator
{
    DateTime Created { get; }
    int? CreatedBy { get; }
}