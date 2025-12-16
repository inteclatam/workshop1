using Intec.Workshop1.Customers.Domain;
using Intec.Workshop1.Customers.Infrastructure;
using Intec.Workshop1.Customers.Primitives;

namespace Intec.Workshop1.Customers.Application.Features.GetCustomerById;

    public class GetCustomerByIdQueryHandler : IQueryHandler<GetCustomerByIdQuery, GetCustomerByIdResponse>
    {
        private readonly ICustomerRepository _repository;

        public GetCustomerByIdQueryHandler(ICustomerRepository repository )
        {
            _repository = repository;
        }

         public async Task<GetCustomerByIdResponse> HandleAsync(GetCustomerByIdQuery request, CancellationToken ct = default)
        {
            var id = new CustomerId(request.Id);
            var customer = await _repository.GetByIdAsync(id,ct);
            if (customer == null)
            {
                
            }

            return new GetCustomerByIdResponse(
                customer!.Name.FullName,
                customer.Email!.Value,
                customer.PhoneNumber!.Value
            );

        }
    }
