using Intec.Workshop1.Customers.Primitives;

namespace Intec.Workshop1.Customers.Application.Features.UpdateCustomer;

public record UpdateCustomerCommand(
    long Id,
    string Email,
    string PhoneNumber) : ICommand<UpdateCustomerResponse>;
