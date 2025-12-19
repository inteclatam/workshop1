using Intec.Workshop1.Customers.Primitives;

namespace Intec.Workshop1.Customers.Application.Features.GetAllCustomers;

public record GetAllCustomersQuery(int Page = 1, int PageSize = 10) : IQuery<GetAllCustomersResponse>;
