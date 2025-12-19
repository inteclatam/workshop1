using Intec.Workshop1.Customers.Primitives;

namespace Intec.Workshop1.Customers.Application.Features.DeleteCustomer;

public record DeleteCustomerCommand(long Id) : ICommand<DeleteCustomerResponse>;
