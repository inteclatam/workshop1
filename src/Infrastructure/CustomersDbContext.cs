using Intec.Workshop1.Customers.Domain;
using Intec.Workshop1.Customers.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Intec.Workshop1.Customers.Infrastructure;

public class CustomersDbContext(DbContextOptions<CustomersDbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<ContactInformation> ContactInformations => Set<ContactInformation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplicar todas las configuraciones del assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomersDbContext).Assembly);

        // Seed inicial de datos
    //    DatabaseSeeder.SeedData(modelBuilder);
    }
}
