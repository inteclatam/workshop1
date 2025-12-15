using Intec.Workshop1.Customers.Primitives;
using Intec.Workshop1.Customers.Infrastructure.Filters;

namespace Intec.Workshop1.Customers.Application.Features.CreateCustomer;

public static class CreateCustomerEndpoint
{
    public static IEndpointRouteBuilder MapCreateCustomerEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/customers", async (
            CreateCustomerRequest request,
            CommandDispatcher dispatcher,
            CancellationToken cancellationToken) =>
        {
            var command = new CreateCustomerCommand(
                request.FirstName,
                request.LastName,
                request.EMail,
                request.PhoneNumber);

            var result = await dispatcher.DispatchAsync<CreateCustomerResponse>(command, cancellationToken);

            return Results.Created($"/api/customers/{result.Id}", result);
        })
        .AddEndpointFilter<ValidationFilter>()
        .WithName("CreateCustomer")
        .WithTags("Customers")
        .Produces<CreateCustomerResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem();

        return endpoints;
    }
}

