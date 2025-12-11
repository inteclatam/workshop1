
using Intec.Workshop1.Customers.Domain;
using Intec.Workshop1.Customers.Infrastructure;
using Intec.Workshop1.Customers.Primitives;

namespace Intec.Workshop1.Customers.Application.Features.CreateCustomer;

public class CreateCustomerCommandHandler:ICommandHandler<CreateCustomerCommand,CreateCustomerResponse>
{
    private readonly ICustomerRepository _repository;

    public CreateCustomerCommandHandler(ICustomerRepository repository)
    {
       _repository = repository; 
    }
    public async Task<CreateCustomerResponse> HandleAsync(CreateCustomerCommand command, CancellationToken ct = default)
    {
        var customer = Customer.Create(command.FirstName, command.LastName, command.EMail, command.PhoneNumber);
        await _repository.AddAsync(customer);
        return new CreateCustomerResponse(customer.Name.FullName, customer.Email.Value);
    }
}