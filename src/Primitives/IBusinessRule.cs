namespace MVCT.Terra.CommonV1.Domain.Primitives;

public interface IBusinessRule
{
    bool IsBroken();
    string Message { get; }
}