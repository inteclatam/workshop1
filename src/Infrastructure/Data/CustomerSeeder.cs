using Bogus;
using Intec.Workshop1.Customers.Domain;
using Intec.Workshop1.Customers.Infrastructure.SnowflakeId;
using Microsoft.EntityFrameworkCore;

namespace Intec.Workshop1.Customers.Infrastructure.Data;

/// <summary>
/// Seeder for generating fake customer data using Bogus and Snowflake ID generator
/// </summary>
public class CustomerSeeder
{
    private readonly CustomersDbContext _context;
    private readonly IIdGenerator _idGenerator;

    public CustomerSeeder(CustomersDbContext context, IIdGenerator idGenerator)
    {
        _context = context;
        _idGenerator = idGenerator;
    }

    /// <summary>
    /// Seeds the database with fake customers
    /// </summary>
    /// <param name="count">Number of customers to generate (default: 50)</param>
    public async Task SeedAsync(int count = 50)
    {
        // Check if customers already exist
        if (await _context.Customers.AnyAsync())
        {
            Console.WriteLine("Database already contains customers. Skipping seed.");
            return;
        }

        Console.WriteLine($"Generating {count} customers with Bogus...");

        // Configure Bogus to generate customers
        var customerFaker = new Faker<Customer>()
            .CustomInstantiator(f =>
            {
                var customerId = _idGenerator.GenerateId();
                var contactId = _idGenerator.GenerateId();

                var firstName = f.Name.FirstName();
                var lastName = f.Name.LastName();
                var email = f.Internet.Email(firstName, lastName);

                // Generate phone number in the format: prefix+number
                var phonePrefix = f.Random.Number(1, 99).ToString();
                var phoneNumber = f.Phone.PhoneNumber("##########");
                var phone = $"{phonePrefix}+{phoneNumber}";

                return Customer.Create(
                    id: customerId,
                    contactId: contactId,
                    firstName: firstName,
                    lastName: lastName,
                    email: email,
                    phoneNumber: phone,
                    userId: null
                );
            });

        // Generate customers
        var customers = customerFaker.Generate(count);

        // Add to database
        await _context.Customers.AddRangeAsync(customers);
        await _context.SaveChangesAsync();

        Console.WriteLine($"Successfully seeded {count} customers!");
        Console.WriteLine($"Sample customer: {customers.First().Name.FullName} - {customers.First().Email?.Value}");
    }

    /// <summary>
    /// Seeds additional contact information for existing customers
    /// </summary>
    /// <param name="maxAdditionalContacts">Maximum additional contacts per customer</param>
    public async Task SeedAdditionalContactsAsync(int maxAdditionalContacts = 2)
    {
        var customers = await _context.Customers
            .Include(c => c.ContactInformations)
            .ToListAsync();

        if (!customers.Any())
        {
            Console.WriteLine("No customers found. Run SeedAsync first.");
            return;
        }

        Console.WriteLine($"Adding additional contacts to {customers.Count} customers...");

        var random = new Random();
        var contactFaker = new Faker();

        foreach (var customer in customers)
        {
            var additionalContactsCount = random.Next(0, maxAdditionalContacts + 1);

            for (int i = 0; i < additionalContactsCount; i++)
            {
                var contactId = _idGenerator.GenerateId();
                var email = contactFaker.Internet.Email();
                var phonePrefix = contactFaker.Random.Number(1, 99).ToString();
                var phoneNumber = contactFaker.Phone.PhoneNumber("##########");
                var phone = $"{phonePrefix}+{phoneNumber}";

                customer.AddContactInformation(
                    contactId: contactId,
                    email: email,
                    phoneNumber: phone,
                    isPrimary: false,
                    userId: null
                );
            }
        }

        await _context.SaveChangesAsync();
        Console.WriteLine($"Successfully added additional contacts!");
    }
}
