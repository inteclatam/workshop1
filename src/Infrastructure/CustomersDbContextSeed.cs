using Intec.Workshop1.Customers.Domain;
using Intec.Workshop1.Customers.Infrastructure.SnowflakeId;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Intec.Workshop1.Customers.Infrastructure;

/// <summary>
/// Seeder for CustomerDbContext using Snowflake ID generator
/// </summary>
public class CustomersDbContextSeed
{
    private readonly CustomersDbContext _context;
    private readonly IIdGenerator _idGenerator;
    private readonly ILogger<CustomersDbContextSeed> _logger;

    public CustomersDbContextSeed(
        CustomersDbContext context,
        IIdGenerator idGenerator,
        ILogger<CustomersDbContextSeed> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _idGenerator = idGenerator ?? throw new ArgumentNullException(nameof(idGenerator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Seeds the database with initial data using Snowflake IDs
    /// </summary>
    public async Task SeedAsync()
    {
        try
        {
            // Ensure database is created
            await _context.Database.MigrateAsync();

            // Check if data already exists
            if (await _context.Customers.AnyAsync())
            {
                _logger.LogInformation("Database already contains data. Skipping seed.");
                return;
            }

            _logger.LogInformation("Starting database seed with Snowflake IDs...");

            // Create sample customers
            var customers = new List<Customer>
            {
                Customer.Create(
                    id: _idGenerator.GenerateId(),
                    contactId: _idGenerator.GenerateId(),
                    firstName: "John",
                    lastName: "Doe",
                    email: "john.doe@example.com",
                    phoneNumber: "+1234567890"
                ),
                Customer.Create(
                    id: _idGenerator.GenerateId(),
                    contactId: _idGenerator.GenerateId(),
                    firstName: "Jane",
                    lastName: "Smith",
                    email: "jane.smith@example.com",
                    phoneNumber: "+1987654321"
                ),
                Customer.Create(
                    id: _idGenerator.GenerateId(),
                    contactId: _idGenerator.GenerateId(),
                    firstName: "Michael",
                    lastName: "Johnson",
                    email: "michael.johnson@example.com",
                    phoneNumber: "+1555123456"
                ),
                Customer.Create(
                    id: _idGenerator.GenerateId(),
                    contactId: _idGenerator.GenerateId(),
                    firstName: "Emily",
                    lastName: "Williams",
                    email: "emily.williams@example.com",
                    phoneNumber: "+1555987654"
                ),
                Customer.Create(
                    id: _idGenerator.GenerateId(),
                    contactId: _idGenerator.GenerateId(),
                    firstName: "David",
                    lastName: "Brown",
                    email: "david.brown@example.com",
                    phoneNumber: "+1555246810"
                )
            };

            // Add additional contact information to some customers
            customers[0].AddContactInformation(
                contactId: _idGenerator.GenerateId(),
                email: "john.doe.work@example.com",
                phoneNumber: "+1234567891",
                isPrimary: false
            );

            customers[1].AddContactInformation(
                contactId: _idGenerator.GenerateId(),
                email: "jane.smith.personal@example.com",
                phoneNumber: "+1987654322",
                isPrimary: false
            );

            // Verify some contacts
            customers[0].VerifyContact(customers[0].PrimaryContact!.Id);
            customers[1].VerifyContact(customers[1].PrimaryContact!.Id);
            customers[2].VerifyContact(customers[2].PrimaryContact!.Id);

            // Add customers to context
            await _context.Customers.AddRangeAsync(customers);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Database seed completed successfully. Added {Count} customers with Snowflake IDs.", customers.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }
}
