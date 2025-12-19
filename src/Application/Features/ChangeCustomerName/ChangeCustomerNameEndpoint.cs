using Intec.Workshop1.Customers.Infrastructure.Filters;
using Intec.Workshop1.Customers.Primitives;

namespace Intec.Workshop1.Customers.Application.Features.ChangeCustomerName;

public static class ChangeCustomerNameEndpoint
{
    public static IEndpointRouteBuilder MapChangeCustomerNameEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPatch("/api/customers/{id}/change-name", async (
                long id,
                ChangeCustomerNameRequest request,
                CommandDispatcher dispatcher,
                CancellationToken cancellationToken) =>
            {
                var command = new ChangeCustomerNameCommand(
                    id,
                    request.FirstName,
                    request.LastName);

                var result = await dispatcher.DispatchAsync<ChangeCustomerNameResponse>(command, cancellationToken);

                return Results.Ok(result);
            })
            .AddEndpointFilter<ValidationFilter>()
            .WithName("ChangeCustomerName")
            .WithTags("Customers")
            .Produces<ChangeCustomerNameResponse>(StatusCodes.Status200OK)
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status404NotFound);

        return endpoints;
    }
}
