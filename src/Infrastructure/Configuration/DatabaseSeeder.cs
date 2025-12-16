using Bogus;
using Intec.Workshop1.Customers.Domain;
using Intec.Workshop1.Customers.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Intec.Workshop1.Customers.Infrastructure.SnowflakeId;

namespace Intec.Workshop1.Customers.Infrastructure.Configuration;

/// <summary>
/// Seeds the database with initial test data using Bogus and Snowflake ID generator.
/// </summary>
public static class DatabaseSeeder
{
    public static void SeedData(ModelBuilder modelBuilder)
    {
        // Seed fijo para reproducibilidad
        Randomizer.Seed = new Random(12345);

        // Inicializar el generador de IDs manualmente para el seeder
        var idGeneratorOptions = new IdGeneratorOptions
        {
            WorkerId = 1,
            DatacenterId = 1
        };
        var idGeneratorPool = new DefaultIdGeneratorPool(idGeneratorOptions);
        var idGenerator = new SnowflakeIdGenerator(idGeneratorPool);

        var customerSeeds = new List<object>();
        var contactSeeds = new List<object>();

        var faker = new Faker("es");

        // Generar 50 customers
        for (int i = 0; i < 50; i++)
        {
            var customerId = idGenerator.GenerateId();
            var firstName = faker.Name.FirstName();
            var lastName = faker.Name.LastName();
            var created = DateTime.SpecifyKind(
                faker.Date.Between(new DateTime(2023, 1, 1), DateTime.UtcNow),
                DateTimeKind.Utc);
            var createdBy = faker.Random.Int(1, 100);

            // Customer seed - use flattened properties for owned entity Name
            customerSeeds.Add(new
            {
                Id = new CustomerId(customerId),
                Name_FirstName = firstName,
                Name_LastName = lastName,
                Created = created,
                CreatedBy = (int?)createdBy,
                LastModified = (DateTime?)null,
                LastModifiedBy = (int?)null,
                IsDeleted = false,
                Deleted = (DateTime?)null,
                DeletedBy = (int?)null
            });

            // Contacto principal
            var primaryContactId = idGenerator.GenerateId();
            var primaryEmail = faker.Internet.Email(firstName, lastName);
            var primaryPhoneNumber = faker.Phone.PhoneNumber("##########");
            var primaryPhonePrefix = faker.Random.Number(1, 99).ToString();

            contactSeeds.Add(new
            {
                Id = primaryContactId,
                Email_Value = primaryEmail,
                PhoneNumber_Number = primaryPhoneNumber,
                PhoneNumber_Prefix = primaryPhonePrefix,
                PhoneNumber_Value = $"{primaryPhonePrefix}{primaryPhoneNumber}",
                IsVerified = faker.Random.Bool(0.7f), // 70% verificados para contacto principal
                IsPrimary = true,
                CustomerId = new CustomerId(customerId)
            });

            // Contactos adicionales (0-2 por customer)
            var additionalContacts = faker.Random.Int(0, 2);
            for (int j = 0; j < additionalContacts; j++)
            {
                var additionalContactId = idGenerator.GenerateId();
                var additionalEmail = faker.Internet.Email();
                var additionalPhoneNumber = faker.Phone.PhoneNumber("##########");
                var additionalPhonePrefix = faker.Random.Number(1, 99).ToString();

                contactSeeds.Add(new
                {
                    Id = additionalContactId,
                    Email_Value = additionalEmail,
                    PhoneNumber_Number = additionalPhoneNumber,
                    PhoneNumber_Prefix = additionalPhonePrefix,
                    PhoneNumber_Value = $"{additionalPhonePrefix}{additionalPhoneNumber}",
                    IsVerified = faker.Random.Bool(0.3f), // 30% verificados
                    IsPrimary = false,
                    CustomerId = new CustomerId(customerId)
                });
            }
        }

        // Aplicar el seed a través de HasData
        modelBuilder.Entity<Customer>().HasData(customerSeeds);
        modelBuilder.Entity<ContactInformation>().HasData(contactSeeds);
    }
}