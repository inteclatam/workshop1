

namespace Intec.Workshop1.Customers.Primitives;

public interface IHaveCreator
{
    DateTime Created { get; }
    int? CreatedBy { get; }
}