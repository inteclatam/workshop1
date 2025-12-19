using Intec.Workshop1.Customers.Domain;
using Intec.Workshop1.Customers.Domain.ValueObjects;
using Intec.Workshop1.Customers.Primitives;
using Microsoft.EntityFrameworkCore;

namespace Intec.Workshop1.Customers.Infrastructure;

public class CustomerRepository : ICustomerRepository
{
    private readonly CustomersDbContext _context;

    public CustomerRepository(CustomersDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Customer?> GetByIdAsync(CustomerId id, CancellationToken cancellationToken = default)
    {
        return await _context.Customers
            .Include(c => c.ContactInformations)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<Customer?> GetByEmailAsync(EMailAddress email, CancellationToken cancellationToken = default)
    {
        return await _context.Customers
            .Include(c => c.ContactInformations)
            .FirstOrDefaultAsync(c => c.ContactInformations.Any(ci => ci.Email == email), cancellationToken);
    }

    public async Task<PagedResult<Customer>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.Customers
            .Include(c => c.ContactInformations)
            .Where(c => !c.IsDeleted)
            .OrderByDescending(c => c.Created);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Customer>(items, page, pageSize, totalCount);
    }

    public async Task AddAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        await _context.Customers.AddAsync(customer, cancellationToken);
    }

    public Task UpdateAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        _context.Customers.Update(customer);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        _context.Customers.Remove(customer);
        return Task.CompletedTask;
    }
}