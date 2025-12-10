

namespace Intec.Workshop1.Customers.Primitives;

public interface IBusinessRule
{
    bool IsBroken();
    string Message { get; }
}