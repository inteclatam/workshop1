namespace Intec.Workshop1.Customers.Infrastructure.Filters;

using FluentValidation;

public class ValidationFilter : IEndpointFilter
{
    private readonly IServiceProvider _serviceProvider;

    public ValidationFilter(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        foreach (var arg in context.Arguments)
        {
            if (arg is null) continue;

            var validatorType = typeof(IValidator<>).MakeGenericType(arg.GetType());
            
            if (_serviceProvider.GetService(validatorType) is not IValidator validator)
                continue;

            var validationContext = new ValidationContext<object>(arg);
            var validationResult = await validator.ValidateAsync(
                validationContext, 
                context.HttpContext.RequestAborted);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );

                return Results.ValidationProblem(errors);
            }
        }

        return await next(context);
    }
}