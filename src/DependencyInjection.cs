using Intec.Workshop1.Customers.Infrastructure;
using Intec.Workshop1.Customers.Infrastructure.SnowflakeId;
using Intec.Workshop1.Customers.Infrastructure.Filters;
using Intec.Workshop1.Customers.Primitives;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Intec.Workshop1.Customers;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Database Context
        services.AddDbContext<CustomersDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
        );

        // Configure IdGenerator (Snowflake)
        var workerId = configuration.GetValue<ushort>("IdGenerator:WorkerId");
        var datacenterId = configuration.GetValue<ushort>("IdGenerator:DatacenterId");

        var idGeneratorOptions = new IdGeneratorOptions
        {
            WorkerId = workerId,
            DatacenterId = datacenterId
        };

        services.AddSingleton<IIdGeneratorPool>(sp => new DefaultIdGeneratorPool(idGeneratorOptions));
        services.AddSingleton<IIdGenerator, SnowflakeIdGenerator>();

        // Repository & Unit of Work
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // CQRS Dispatchers
        services.AddScoped<CommandDispatcher>();
        services.AddScoped<QueryDispatcher>();

        // Register all Command Handlers
        RegisterHandlers(services, typeof(ICommandHandler<,>));

        // Register all Query Handlers
        RegisterHandlers(services, typeof(IQueryHandler<,>));

        // FluentValidation - Register all validators from assembly
        services.AddValidatorsFromAssemblyContaining<DependencyInjection>(ServiceLifetime.Scoped);

        // Filters
        services.AddScoped<ValidationFilter>();

        return services;
    }

    private static void RegisterHandlers(IServiceCollection services, Type handlerInterfaceType)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        var handlers = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract)
            .SelectMany(t => t.GetInterfaces(), (type, iface) => new { Type = type, Interface = iface })
            .Where(x => x.Interface.IsGenericType && x.Interface.GetGenericTypeDefinition() == handlerInterfaceType)
            .ToList();

        foreach (var handler in handlers)
        {
            services.AddScoped(handler.Interface, handler.Type);
        }
    }
}