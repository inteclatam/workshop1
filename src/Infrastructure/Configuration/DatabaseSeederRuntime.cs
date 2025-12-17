using Bogus;
using Intec.Workshop1.Customers.Domain;
using Intec.Workshop1.Customers.Infrastructure.SnowflakeId;
using Microsoft.EntityFrameworkCore;

namespace Intec.Workshop1.Customers.Infrastructure.Configuration;

public static class DatabaseSeederRuntime
{
    public static async Task SeedAsync(CustomersDbContext db, CancellationToken ct = default)
    {
        // Aplica migraciones antes de seedear
        await db.Database.MigrateAsync(ct);

        // Si ya hay data, no duplicar
        if (await db.Customers.AsNoTracking().AnyAsync(ct))
            return;

        Randomizer.Seed = new Random(12345);

        // Snowflake
        var opts = new IdGeneratorOptions { WorkerId = 1, DatacenterId = 1 };
        var pool = new DefaultIdGeneratorPool(opts);
        var idGen = new SnowflakeIdGenerator(pool);

        var faker = new Faker("es");
        var customers = new List<Customer>(50);

        for (int i = 0; i < 50; i++)
        {
            var customerId = idGen.GenerateId();
            var primaryContactId = idGen.GenerateId();

            var firstName = faker.Name.FirstName();
            var lastName = faker.Name.LastName();

            // Email UNIQUE (por el índice unique)
            var email = $"{firstName}.{lastName}.{customerId}@seed.local".ToLowerInvariant();

            // Phone en formato requerido por el dominio: "57+3001234567"
            var prefix = faker.Random.Number(1, 99).ToString();
            var number = faker.Random.ReplaceNumbers("##########");
            var phone = $"{prefix}+{number}";

            // Crea el customer + contacto principal
            var customer = Customer.Create(
                id: customerId,
                contactId: primaryContactId,
                firstName: firstName,
                lastName: lastName,
                email: email,
                phoneNumber: phone,
                userId: 1);

            // Contactos extra (0-2)
            var extras = faker.Random.Int(0, 2);
            for (int j = 0; j < extras; j++)
            {
                var contactId = idGen.GenerateId();
                var extraEmail = $"{faker.Internet.UserName().ToLowerInvariant()}.{contactId}@seed.local";

                var pfx = faker.Random.Number(1, 99).ToString();
                var num = faker.Random.ReplaceNumbers("##########");
                var extraPhone = $"{pfx}+{num}";

                customer.AddContactInformation(
                    contactId: contactId,
                    email: extraEmail,
                    phoneNumber: extraPhone,
                    isPrimary: false,
                    userId: 1);
            }

            customers.Add(customer);
        }

        db.Customers.AddRange(customers);
        await db.SaveChangesAsync(ct);
    }
}