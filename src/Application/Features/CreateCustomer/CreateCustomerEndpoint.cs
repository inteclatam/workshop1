
using Intec.Workshop1.Customers.Primitives;

namespace Intec.Workshop1.Customers.Application.Features.CreateCustomer;

public static class CreateCustomerEndpoint
{
    private static async Task<IResult> Handle(CreateCustomerRequest request,CommandDispatcher dispatcher, CancellationToken cancellationToken)
    {
        var command = new CreateCustomerCommand(request.FirstName,request.LastName,request.EMail, request.PhoneNumber);
      var result=  await dispatcher.DispatchAsync(command, cancellationToken);

      return Results.Created($"/customers/{result.Id}",result);
    }
}