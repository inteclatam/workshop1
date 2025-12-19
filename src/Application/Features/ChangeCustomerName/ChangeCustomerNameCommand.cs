using Intec.Workshop1.Customers.Primitives;

namespace Intec.Workshop1.Customers.Application.Features.ChangeCustomerName;

public record ChangeCustomerNameCommand(
    long Id,
    string FirstName,
    string LastName) : ICommand<ChangeCustomerNameResponse>;
