using Intec.Workshop1.Customers.Primitives;

namespace Intec.Workshop1.Customers.Application.Features.GetCustomerById;

public record  GetCustomerByIdQuery (long Id) : IQuery<GetCustomerByIdQueryResponse>;