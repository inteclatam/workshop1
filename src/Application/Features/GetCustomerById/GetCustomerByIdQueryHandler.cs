using Intec.Workshop1.Customers.Domain;
using Intec.Workshop1.Customers.Infrastructure;
using Intec.Workshop1.Customers.Primitives;

namespace Intec.Workshop1.Customers.Application.Features.GetCustomerById;

/*
public class GetCustomerByIdQueryHandler:IQueryHandler<GetCustomerByIdQuery,GetCustomerByIdQueryResponse>
{
    public GetCustomerByIdQueryHandler(ICustomerRepository repository,)
    {
        _repository = repository;
    }
    private readonly ICustomerRepository _repository;
    public async Task<GetCustomerByIdQueryResponse> HandleAsync(GetCustomerByIdQuery query, CancellationToken ct = default)
    {
        var customerId = new CustomerId(query.Id);
        var customer = await _repository.GetByIdAsync(customerId, ct);
        var response = new GetCustomerByIdQueryResponse
            (FullName:customer!.Name.FullName,customer.Email!.Value, customer.PhoneNumber!.Value);

        return response;
    }
}*/