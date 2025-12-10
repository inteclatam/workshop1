namespace MVCT.Terra.CommonV1.Domain.Primitives;

public interface IHaveAudit : IHaveCreator
{
    DateTime? LastModified { get; }
    int? LastModifiedBy { get; }
}