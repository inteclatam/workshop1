using Intec.Workshop1.Customers.Infrastructure;
using Intec.Workshop1.Customers.Primitives;

namespace Intec.Workshop1.Customers.Application.Features.GetAllCustomers;

public class GetAllCustomersQueryHandler : IQueryHandler<GetAllCustomersQuery, GetAllCustomersResponse>
{
    private readonly ICustomerRepository _repository;

    public GetAllCustomersQueryHandler(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetAllCustomersResponse> HandleAsync(GetAllCustomersQuery request, CancellationToken ct = default)
    {
        var pagedResult = await _repository.GetAllAsync(request.Page, request.PageSize, ct);

        var customerDtos = pagedResult.Items.Select(c => new CustomerDto(
            c.Id.Value,
            c.Name.FullName,
            c.Email?.Value ?? string.Empty,
            c.PhoneNumber?.Value ?? string.Empty,
            c.Created,
            c.IsDeleted
        ));

        return new GetAllCustomersResponse(
            customerDtos,
            pagedResult.Page,
            pagedResult.PageSize,
            pagedResult.TotalCount,
            pagedResult.TotalPages,
            pagedResult.HasPreviousPage,
            pagedResult.HasNextPage
        );
    }
}
