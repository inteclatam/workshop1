using Intec.Workshop1.Customers.Domain;
using Microsoft.EntityFrameworkCore;
using ContactInformation = Intec.Workshop1.Customers.Infrastructure.Configuration.ContactInformation;

namespace Intec.Workshop1.Customers.Infrastructure;

public class CustomersDbContext:DbContext
{
    public CustomersDbContext(DbContextOptions<CustomersDbContext> options) : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<ContactInformation> ContactInformations => Set<ContactInformation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomersDbContext).Assembly);
        
    }
}
