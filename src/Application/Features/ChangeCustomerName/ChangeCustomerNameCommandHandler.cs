using Intec.Workshop1.Customers.Domain;
using Intec.Workshop1.Customers.Infrastructure;
using Intec.Workshop1.Customers.Primitives;

namespace Intec.Workshop1.Customers.Application.Features.ChangeCustomerName;

public class ChangeCustomerNameCommandHandler : ICommandHandler<ChangeCustomerNameCommand, ChangeCustomerNameResponse>
{
    private readonly ICustomerRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ChangeCustomerNameCommandHandler> _logger;

    public ChangeCustomerNameCommandHandler(
        ICustomerRepository repository,
        IUnitOfWork unitOfWork,
        ILogger<ChangeCustomerNameCommandHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ChangeCustomerNameResponse> HandleAsync(ChangeCustomerNameCommand command, CancellationToken ct = default)
    {
        var customerId = new CustomerId(command.Id);
        var customer = await _repository.GetByIdAsync(customerId, ct);

        if (customer == null)
        {
            throw new InvalidOperationException($"Customer with ID {command.Id} not found");
        }

        _logger.LogInformation("Changing name for customer with ID: {CustomerId}", command.Id);

        // Use domain method to update name (this will raise CustomerNameChangedEvent)
        customer.UpdateName(command.FirstName, command.LastName);

        await _repository.UpdateAsync(customer, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        _logger.LogInformation("Customer name changed successfully for ID: {CustomerId}", command.Id);

        return new ChangeCustomerNameResponse(
            customer.Id.Value,
            customer.Name.FullName,
            customer.Name.FirstName,
            customer.Name.LastName);
    }
}
