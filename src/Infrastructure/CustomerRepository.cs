using Intec.Workshop1.Customers.Domain;
using Intec.Workshop1.Customers.Domain.ValueObjects;

namespace Intec.Workshop1.Customers.Infrastructure;

public class CustomerRepository:ICustomerRepository
{
    public Task<Customer?> GetByIdAsync(CustomerId id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Customer?> GetByEmailAsync(EMailAddress email, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}