
using Intec.Workshop1.Customers.Primitives;

namespace Intec.Workshop1.Customers.Application.Features.CreateCustomer;

public static class CreateCustomerEndpoint
{
    public static IEndpointRouteBuilder MapCreateCustomerEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/customers",  (
    CreateCustomerRequest request
    //CommandDispatcher dispatcher,
    //CancellationToken cancellationToken
    ) =>
        {
            var command = new CreateCustomerCommand(
                request.FirstName,
                request.LastName,
                request.EMail,
                request.PhoneNumber);

       //    var result = await dispatcher.DispatchAsync(command, cancellationToken);

            return Results.Created($"/customers/1", 
                new CreateCustomerResponse("edward","edward@mail.com",1));
        }
      );
        return endpoints;
    }
}

