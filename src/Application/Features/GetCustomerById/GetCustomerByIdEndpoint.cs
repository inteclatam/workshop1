using Ardalis.GuardClauses;
using Intec.Workshop1.Customers.Infrastructure.Filters;
using Intec.Workshop1.Customers.Primitives;

namespace Intec.Workshop1.Customers.Application.Features.GetCustomerById;


public static class GetCustomerByIdEndpoint
{
    public static IEndpointRouteBuilder MapGetCustomerByIdEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/customers/{id}", async (
                long id,
                QueryDispatcher dispatcher,
                CancellationToken cancellationToken) =>
            {
                
                
                
                Guard.Against.NegativeOrZero(id, nameof(id));

                var query = new GetCustomerByIdQuery(id);
                var result = await dispatcher.DispatchAsync<GetCustomerByIdResponse>(query, cancellationToken);

                return result is null
                    ? Results.NotFound()
                    : Results.Ok(result);
            })
            .AddEndpointFilter<ValidationFilter>()
            .WithName("GetCustomerById")
            .WithTags("Customers")
            .Produces<GetCustomerByIdResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        return endpoints;
    }
}

