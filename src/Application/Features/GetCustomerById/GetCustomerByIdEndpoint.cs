using Intec.Workshop1.Customers.Application.Features.CreateCustomer;
using Intec.Workshop1.Customers.Infrastructure.Filters;
using Intec.Workshop1.Customers.Primitives;

namespace Intec.Workshop1.Customers.Application.Features.GetCustomerById;


public static class GetCustomerByIdEndpoint
{
    public static IEndpointRouteBuilder MapGetCustomerByIdEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/customers/{id}", async(
                GetCustomerByIdQuery query,
                CommandDispatcher dispatcher,
                CancellationToken cancellationToken) =>
            {
          

         //       var result = await dispatcher.DispatchAsync<GetCustomerByIdQueryResponse>(query, cancellationToken);
//if(result==null) return Results.NotFound();

                return Results.Ok();
            })
            .AddEndpointFilter<ValidationFilter>()
            .WithName("CreateCustomer")
            .WithTags("Customers")
            .Produces<CreateCustomerResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem();

        return endpoints;
    }
}

