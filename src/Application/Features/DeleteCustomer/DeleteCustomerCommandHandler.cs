using Intec.Workshop1.Customers.Domain;
using Intec.Workshop1.Customers.Infrastructure;
using Intec.Workshop1.Customers.Primitives;

namespace Intec.Workshop1.Customers.Application.Features.DeleteCustomer;

public class DeleteCustomerCommandHandler : ICommandHandler<DeleteCustomerCommand, DeleteCustomerResponse>
{
    private readonly ICustomerRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteCustomerCommandHandler> _logger;

    public DeleteCustomerCommandHandler(
        ICustomerRepository repository,
        IUnitOfWork unitOfWork,
        ILogger<DeleteCustomerCommandHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<DeleteCustomerResponse> HandleAsync(DeleteCustomerCommand command, CancellationToken ct = default)
    {
        var customerId = new CustomerId(command.Id);
        var customer = await _repository.GetByIdAsync(customerId, ct);

        if (customer == null)
        {
            throw new InvalidOperationException($"Customer with ID {command.Id} not found");
        }

        if (customer.IsDeleted)
        {
            _logger.LogWarning("Customer with ID {CustomerId} is already deleted", command.Id);
            return new DeleteCustomerResponse(customer.Id.Value, customer.IsDeleted, customer.Deleted);
        }

        _logger.LogInformation("Soft deleting customer with ID: {CustomerId}", command.Id);

        // Use domain method for soft delete (this will raise CustomerDeletedEvent)
        customer.SoftDelete();

        await _repository.UpdateAsync(customer, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        _logger.LogInformation("Customer soft deleted successfully with ID: {CustomerId}", command.Id);

        return new DeleteCustomerResponse(
            customer.Id.Value,
            customer.IsDeleted,
            customer.Deleted);
    }
}
