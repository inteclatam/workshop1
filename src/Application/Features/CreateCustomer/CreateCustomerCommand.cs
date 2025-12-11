

using Intec.Workshop1.Customers.Primitives;

namespace Intec.Workshop1.Customers.Application.Features.CreateCustomer;

public record CreateCustomerCommand(string FirstName, string LastName, string EMail, string PhoneNumber)
    : ICommand<CreateCustomerResponse>;
