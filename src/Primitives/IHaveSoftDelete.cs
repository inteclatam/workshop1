
namespace Intec.Workshop1.Customers.Primitives;

public interface IHaveSoftDelete
{
    bool IsDeleted { get; set; }
    DateTime? Deleted { get;set; }
    int? DeletedBy { get;set; }
}