using Intec.Workshop1.Customers.Infrastructure.Filters;
using Intec.Workshop1.Customers.Primitives;

namespace Intec.Workshop1.Customers.Application.Features.DeleteCustomer;

public static class DeleteCustomerEndpoint
{
    public static IEndpointRouteBuilder MapDeleteCustomerEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete("/api/customers/{id}", async (
                long id,
                CommandDispatcher dispatcher,
                CancellationToken cancellationToken) =>
            {
                var command = new DeleteCustomerCommand(id);
                var result = await dispatcher.DispatchAsync<DeleteCustomerResponse>(command, cancellationToken);

                return Results.Ok(result);
            })
            .AddEndpointFilter<ValidationFilter>()
            .WithName("DeleteCustomer")
            .WithTags("Customers")
            .Produces<DeleteCustomerResponse>(StatusCodes.Status200OK)
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status404NotFound);

        return endpoints;
    }
}
