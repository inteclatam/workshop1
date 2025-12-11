
using Intec.Workshop1.Customers.Domain;
using Intec.Workshop1.Customers.Infrastructure;
using Intec.Workshop1.Customers.Infrastructure.SnowflakeId;
using Intec.Workshop1.Customers.Primitives;

namespace Intec.Workshop1.Customers.Application.Features.CreateCustomer;

public class CreateCustomerCommandHandler:ICommandHandler<CreateCustomerCommand,CreateCustomerResponse>
{
    private readonly ICustomerRepository _repository;
    private readonly IIdGenerator _idGenerator;

    public CreateCustomerCommandHandler(ICustomerRepository repository, IIdGenerator idGenerator)
    {
       _repository = repository;
       _idGenerator = idGenerator;
    }

    public async Task<CreateCustomerResponse> HandleAsync(CreateCustomerCommand command, CancellationToken ct = default)
    {
        var customerId = _idGenerator.GenerateId();
        var contactId = _idGenerator.GenerateId();

        var customer = Customer.Create(
            customerId,
            contactId,
            command.FirstName,
            command.LastName,
            command.EMail,
            command.PhoneNumber);

        await _repository.AddAsync(customer);
        return new CreateCustomerResponse(customer.Name.FullName, customer.Email!.Value, customerId);
    }
}