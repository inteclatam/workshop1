using Intec.Workshop1.Customers.Infrastructure.Data;

namespace Intec.Workshop1.Customers.Application.Features.SeedData;

public static class SeedDataEndpoint
{
    public static IEndpointRouteBuilder MapSeedDataEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/seed/customers", async (
            CustomerSeeder seeder,
            int? count,
            CancellationToken cancellationToken) =>
        {
            await seeder.SeedAsync(count ?? 50);

            return Results.Ok(new
            {
                Message = $"Successfully seeded {count ?? 50} customers with contact information",
                Count = count ?? 50
            });
        })
        .WithName("SeedCustomers")
        .WithTags("Development")
        .Produces(StatusCodes.Status200OK)
        .WithDescription("Seeds the database with fake customer data using Bogus and Snowflake ID generator");

        endpoints.MapPost("/api/seed/additional-contacts", async (
            CustomerSeeder seeder,
            int? maxAdditionalContacts,
            CancellationToken cancellationToken) =>
        {
            await seeder.SeedAdditionalContactsAsync(maxAdditionalContacts ?? 2);

            return Results.Ok(new
            {
                Message = "Successfully added additional contacts to existing customers",
                MaxContactsPerCustomer = maxAdditionalContacts ?? 2
            });
        })
        .WithName("SeedAdditionalContacts")
        .WithTags("Development")
        .Produces(StatusCodes.Status200OK)
        .WithDescription("Adds additional contact information to existing customers");

        return endpoints;
    }
}
