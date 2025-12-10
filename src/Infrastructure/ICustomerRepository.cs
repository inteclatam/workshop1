using Intec.Workshop1.Customers.Domain;
using Intec.Workshop1.Customers.Domain.ValueObjects;

namespace Intec.Workshop1.Customers.Infrastructure;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(CustomerId id, CancellationToken cancellationToken = default);

    Task<Customer?> GetByEmailAsync(EMailAddress email, CancellationToken cancellationToken = default);

    Task AddAsync(Customer customer, CancellationToken cancellationToken = default);

    Task UpdateAsync(Customer customer, CancellationToken cancellationToken = default);

    Task DeleteAsync(Customer customer, CancellationToken cancellationToken = default);
}