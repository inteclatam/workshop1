using Bogus;
using Microsoft.EntityFrameworkCore;

namespace Intec.Workshop1.Customers.Infrastructure.Configuration;

public static class DatabaseSeeder
{
    public static void SeedData(ModelBuilder modelBuilder)
    {
        // Seed fijo para reproducibilidad
        Randomizer.Seed = new Random(12345);

        var customerId = 1L;
        var contactId = 1L;
        var customerSeeds = new List<object>();
        var contactSeeds = new List<object>();

        var faker = new Faker("es");

        // Generar 50 customers
        for (int i = 0; i < 50; i++)
        {
            var id = customerId++;
            var firstName = faker.Name.FirstName();
            var lastName = faker.Name.LastName();
            var created = faker.Date.Between(new DateTime(2023, 1, 1), DateTime.UtcNow);
            var createdBy = faker.Random.Int(1, 100);

            // Customer seed
            customerSeeds.Add(new
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                Created = created,
                CreatedBy = (int?)createdBy,
                LastModified = (DateTime?)null,
                LastModifiedBy = (int?)null,
                IsDeleted = false,
                Deleted = (DateTime?)null,
                DeletedBy = (int?)null
            });

            // Contacto principal
            var primaryEmail = faker.Internet.Email(firstName, lastName);
            var primaryPhoneNumber = faker.Phone.PhoneNumber("##########");
            var primaryPhonePrefix = faker.Random.Number(1, 99).ToString();

            contactSeeds.Add(new
            {
                Id = contactId++,
                Email = primaryEmail,
                PhoneNumber = primaryPhoneNumber,
                PhonePrefix = primaryPhonePrefix,
                PhoneValue = $"{primaryPhonePrefix}{primaryPhoneNumber}",
                IsVerified = faker.Random.Bool(0.7f), // 70% verificados para contacto principal
                IsPrimary = true,
                CustomerId = id
            });

            // Contactos adicionales (0-2 por customer)
            var additionalContacts = faker.Random.Int(0, 2);
            for (int j = 0; j < additionalContacts; j++)
            {
                var additionalEmail = faker.Internet.Email();
                var additionalPhoneNumber = faker.Phone.PhoneNumber("##########");
                var additionalPhonePrefix = faker.Random.Number(1, 99).ToString();

                contactSeeds.Add(new
                {
                    Id = contactId++,
                    Email = additionalEmail,
                    PhoneNumber = additionalPhoneNumber,
                    PhonePrefix = additionalPhonePrefix,
                    PhoneValue = $"{additionalPhonePrefix}{additionalPhoneNumber}",
                    IsVerified = faker.Random.Bool(0.3f), // 30% verificados
                    IsPrimary = false,
                    CustomerId = id
                });
            }
        }

        // Aplicar el seed a través de HasData
        modelBuilder.Entity("Intec.Workshop1.Customers.Domain.Customer", b =>
        {
            b.HasData(customerSeeds);
        });

        modelBuilder.Entity("Intec.Workshop1.Customers.Domain.ContactInformation", b =>
        {
            b.HasData(contactSeeds);
        });
    }
}
