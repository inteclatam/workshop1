

namespace Intec.Workshop1.Customers.Primitives;

public interface IHaveAudit : IHaveCreator
{
    DateTime? LastModified { get; }
    int? LastModifiedBy { get; }
}