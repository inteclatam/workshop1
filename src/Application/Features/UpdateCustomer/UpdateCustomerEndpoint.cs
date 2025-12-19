using Intec.Workshop1.Customers.Infrastructure.Filters;
using Intec.Workshop1.Customers.Primitives;

namespace Intec.Workshop1.Customers.Application.Features.UpdateCustomer;

public static class UpdateCustomerEndpoint
{
    public static IEndpointRouteBuilder MapUpdateCustomerEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut("/api/customers/{id}", async (
                long id,
                UpdateCustomerRequest request,
                CommandDispatcher dispatcher,
                CancellationToken cancellationToken) =>
            {
                var command = new UpdateCustomerCommand(
                    id,
                    request.Email,
                    request.PhoneNumber);

                var result = await dispatcher.DispatchAsync<UpdateCustomerResponse>(command, cancellationToken);

                return Results.Ok(result);
            })
            .AddEndpointFilter<ValidationFilter>()
            .WithName("UpdateCustomer")
            .WithTags("Customers")
            .Produces<UpdateCustomerResponse>(StatusCodes.Status200OK)
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status404NotFound);

        return endpoints;
    }
}
