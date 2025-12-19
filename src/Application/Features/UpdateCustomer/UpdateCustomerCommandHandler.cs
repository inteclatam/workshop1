using Intec.Workshop1.Customers.Domain;
using Intec.Workshop1.Customers.Infrastructure;
using Intec.Workshop1.Customers.Primitives;

namespace Intec.Workshop1.Customers.Application.Features.UpdateCustomer;

public class UpdateCustomerCommandHandler : ICommandHandler<UpdateCustomerCommand, UpdateCustomerResponse>
{
    private readonly ICustomerRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateCustomerCommandHandler> _logger;

    public UpdateCustomerCommandHandler(
        ICustomerRepository repository,
        IUnitOfWork unitOfWork,
        ILogger<UpdateCustomerCommandHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<UpdateCustomerResponse> HandleAsync(UpdateCustomerCommand command, CancellationToken ct = default)
    {
        var customerId = new CustomerId(command.Id);
        var customer = await _repository.GetByIdAsync(customerId, ct);

        if (customer == null)
        {
            throw new InvalidOperationException($"Customer with ID {command.Id} not found");
        }

        _logger.LogInformation("Updating customer with ID: {CustomerId}", command.Id);

        // Update primary contact information
        var primaryContact = customer.PrimaryContact;
        if (primaryContact != null)
        {
            customer.UpdateContactEmail(primaryContact.Id, command.Email);
            customer.UpdateContactPhoneNumber(primaryContact.Id, command.PhoneNumber);
        }
        else
        {
            throw new InvalidOperationException($"Customer with ID {command.Id} has no primary contact");
        }

        await _repository.UpdateAsync(customer, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        _logger.LogInformation("Customer updated successfully with ID: {CustomerId}", command.Id);

        return new UpdateCustomerResponse(
            customer.Id.Value,
            customer.Name.FullName,
            customer.Email!.Value,
            customer.PhoneNumber!.Value);
    }
}
